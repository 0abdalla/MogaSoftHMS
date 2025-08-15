using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.DailyRestrictions;
using Hospital_MS.Core.Contracts.SupplyReceipts;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Hospital_MS.Services.HMS;
public class SupplyReceiptService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper, IDailyRestrictionService dailyRestrictionService) : ISupplyReceiptService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISQLHelper _sQLHelper = sQLHelper;
    private readonly IDailyRestrictionService _dailyRestrictionService = dailyRestrictionService;

    public async Task<ErrorResponseModel<PartialDailyRestrictionResponse>> CreateSupplyReceiptAsync(SupplyReceiptRequest request, CancellationToken cancellationToken = default)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var treasury = await _unitOfWork.Repository<Treasury>()
                .GetAll(x => x.Id == request.TreasuryId && x.IsActive)
                .FirstOrDefaultAsync(cancellationToken);

            if (treasury == null)
                return ErrorResponseModel<PartialDailyRestrictionResponse>.Failure(GenericErrors.NotFound, null);

            var account = await _unitOfWork.Repository<AccountTree>()
                .GetAll(x => x.AccountId == request.AccountId)
                .FirstOrDefaultAsync(cancellationToken);

            if (account == null)
                return ErrorResponseModel<PartialDailyRestrictionResponse>.Failure(GenericErrors.NotFound, null);


            var supplyReceipt = new SupplyReceipt
            {
                Date = request.Date,
                ReceivedFrom = request.ReceivedFrom,
                Amount = request.Amount,
                Description = request.Description,
                CostCenterId = request.CostCenterId,
                TreasuryId = request.TreasuryId,
                AccountId = request.AccountId
            };

            await _unitOfWork.Repository<SupplyReceipt>().AddAsync(supplyReceipt, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);


            var lastMovement = await _unitOfWork.Repository<TreasuryMovement>()
                .GetAll(x => x.TreasuryId == request.TreasuryId && x.IsClosed == false)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            var treasuryOperation = new TreasuryOperation
            {
                Date = request.Date,
                Amount = request.Amount,
                Description = request.Description,
                TreasuryId = request.TreasuryId,
                ReceivedFrom = request.ReceivedFrom,
                TransactionType = TransactionType.Credit,
                DocumentNumber = supplyReceipt.Id.ToString(),
                AccountId = request.AccountId,
                TreasuryMovementId = lastMovement?.Id,
            };

            lastMovement!.TotalCredits += request.Amount;
            lastMovement.Balance += request.Amount;

            _unitOfWork.Repository<TreasuryMovement>().Update(lastMovement);

            await _unitOfWork.Repository<TreasuryOperation>().AddAsync(treasuryOperation, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            var dailyRestriction = new DailyRestriction
            {
                RestrictionNumber = await _dailyRestrictionService.GenerateRestrictionNumberAsync(cancellationToken),
                DocumentNumber = treasuryOperation.Id.ToString(),
                RestrictionTypeId = null,
                IsActive = true,
                AccountingGuidanceId = 1, // المخازن
                RestrictionDate = request.Date,
                Description = request.Description,
                Details =
                [
                    new DailyRestrictionDetail
                    {
                        AccountId = request.AccountId,
                        CostCenterId = request.CostCenterId,
                        Credit = request.Amount,
                        Note = null,
                        Debit = 0,
                        From = treasury.Name,
                        To = account.NameAR,

                    }
                ]
            };

            await _unitOfWork.Repository<DailyRestriction>().AddAsync(dailyRestriction, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // assign daily restriction to supply receipt

            supplyReceipt.DailyRestrictionId = dailyRestriction.Id;
            _unitOfWork.Repository<SupplyReceipt>().Update(supplyReceipt);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            var response = new PartialDailyRestrictionResponse
            {
                Id = supplyReceipt.Id,
                AccountingGuidanceName = _unitOfWork.Repository<AccountingGuidance>().GetAll(x => x.Id == dailyRestriction.AccountingGuidanceId).FirstOrDefault().Name,
                Amount = request.Amount,
                From = treasury.Name,
                To = account.NameAR,
                RestrictionDate = dailyRestriction.RestrictionDate,
                RestrictionNumber = dailyRestriction.RestrictionNumber
            };

            return ErrorResponseModel<PartialDailyRestrictionResponse>.Success(GenericErrors.AddSuccess, response);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return ErrorResponseModel<PartialDailyRestrictionResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> DeleteSupplyReceiptAsync(int id, CancellationToken cancellationToken = default)
    {

        try
        {
            var supplyReceipt = await _unitOfWork.Repository<SupplyReceipt>()
                .GetAll(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (supplyReceipt == null)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }

            supplyReceipt.IsActive = false;

            _unitOfWork.Repository<SupplyReceipt>().Update(supplyReceipt);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, supplyReceipt.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<SupplyReceiptResponse>> GetSupplyReceiptByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var supplyReceipt = await _unitOfWork.Repository<SupplyReceipt>()
                .GetAll(x => x.Id == id)
                .Include(x => x.CostCenter)
                .Include(x => x.Treasury)
                .Include(x => x.Account)
                .Include(x => x.DailyRestriction)
                    .ThenInclude(dr => dr.AccountingGuidance)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(cancellationToken);

            if (supplyReceipt == null)
            {
                return ErrorResponseModel<SupplyReceiptResponse>.Failure(GenericErrors.NotFound);
            }

            var response = new SupplyReceiptResponse
            {
                Id = supplyReceipt.Id,
                Date = supplyReceipt.Date,
                ReceivedFrom = supplyReceipt.ReceivedFrom,
                AccountNumber = supplyReceipt.Account.AccountNumber,
                AccountId = supplyReceipt.Account.AccountId,
                Amount = supplyReceipt.Amount,
                Description = supplyReceipt.Description,
                CostCenterId = supplyReceipt.CostCenterId,
                TreasuryId = supplyReceipt.TreasuryId,
                CostCenterName = supplyReceipt.CostCenter?.NameAR ?? supplyReceipt.CostCenter?.NameEN ?? "",
                TreasuryName = supplyReceipt.Treasury?.Name,
                Audit = new Core.Contracts.Common.AuditResponse
                {
                    CreatedBy = supplyReceipt.CreatedBy.UserName,
                    CreatedOn = supplyReceipt.CreatedOn,
                    UpdatedBy = supplyReceipt.UpdatedBy?.UserName,
                    UpdatedOn = supplyReceipt.UpdatedOn,
                },
                DailyRestriction = new PartialDailyRestrictionResponse
                {
                    Id = supplyReceipt.DailyRestriction?.Id ?? 0,
                    RestrictionNumber = supplyReceipt.DailyRestriction?.RestrictionNumber ?? string.Empty,
                    RestrictionDate = supplyReceipt.DailyRestriction?.RestrictionDate ?? DateOnly.MinValue,
                    AccountingGuidanceName = supplyReceipt.DailyRestriction?.AccountingGuidance?.Name ?? "",
                    Amount = supplyReceipt.Amount,
                    From = supplyReceipt.Treasury?.Name,
                    To = supplyReceipt.Account?.NameAR
                }
            };
            return ErrorResponseModel<SupplyReceiptResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<SupplyReceiptResponse>.Failure(GenericErrors.TransFailed);
        }

    }

    public async Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
    {
        try
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@ReceiptNumber",
                    pagingFilter.FilterList?.FirstOrDefault(f => f.CategoryName == "ReceiptNumber")?.ItemValue
                    ?? (object)DBNull.Value),

                new SqlParameter("@Date",
                    pagingFilter.FilterList?.FirstOrDefault(f => f.CategoryName == "Date")?.ItemValue
                    ?? (object)DBNull.Value),

                new SqlParameter("@TreasuryId",
                    pagingFilter.FilterList?.FirstOrDefault(f => f.CategoryName == "Treasury")?.ItemValue
                    ?? (object)DBNull.Value),

                new SqlParameter("@TreasuryNumber",
                    pagingFilter.FilterList?.FirstOrDefault(f => f.CategoryName == "TreasuryNumber")?.ItemValue
                    ?? (object)DBNull.Value),

                new SqlParameter("@SortOrder",
                    pagingFilter.FilterList?.FirstOrDefault(f => f.CategoryName == "SortOrder")?.ItemValue
                    ?? "ASC"),

                new SqlParameter("@CurrentPage", pagingFilter.CurrentPage),
                new SqlParameter("@PageSize", pagingFilter.PageSize),

                new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value)

            };

            var dt = await _sQLHelper.ExecuteDataTableAsync("[finance].[SP_GetSupplyReceipts]", parameters.ToArray());

            int totalCount = dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["TotalCount"]) : 0;

            return PagedResponseModel<DataTable>.Success(
                GenericErrors.GetSuccess,
                totalCount,
                dt
            );
        }
        catch (Exception)
        {
            return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateSupplyReceiptAsync(int id, SupplyReceiptRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var supplyReceipt = await _unitOfWork.Repository<SupplyReceipt>()
                .GetAll(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (supplyReceipt == null)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }

            supplyReceipt.Date = request.Date;
            supplyReceipt.ReceivedFrom = request.ReceivedFrom;
            supplyReceipt.AccountId = request.AccountId;
            supplyReceipt.Amount = request.Amount;
            supplyReceipt.Description = request.Description;
            supplyReceipt.CostCenterId = request.CostCenterId;
            supplyReceipt.TreasuryId = request.TreasuryId;

            _unitOfWork.Repository<SupplyReceipt>().Update(supplyReceipt);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, supplyReceipt.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }

    }
}
