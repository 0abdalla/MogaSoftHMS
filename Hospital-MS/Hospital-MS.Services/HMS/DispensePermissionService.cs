using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.DailyRestrictions;
using Hospital_MS.Core.Contracts.DispensePermission;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Hospital_MS.Services.HMS;
public class DispensePermissionService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper, IDailyRestrictionService dailyRestrictionService) : IDispensePermissionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISQLHelper _sQLHelper = sQLHelper;
    private readonly IDailyRestrictionService _dailyRestrictionService = dailyRestrictionService;

    public async Task<ErrorResponseModel<PartialDailyRestrictionResponse>> CreateAsync(DispensePermissionRequest request, CancellationToken cancellationToken = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var treasury = await _unitOfWork.Repository<Treasury>()
                .GetByIdAsync(request.TreasuryId, cancellationToken);

            if (treasury is null)
                return ErrorResponseModel<PartialDailyRestrictionResponse>.Failure(new Error("الخزينه غير موجوده", Status.NotFound));

            var account = await _unitOfWork.Repository<AccountTree>()
                .GetByIdAsync(request.AccountId, cancellationToken);

            if (account is null)
                return ErrorResponseModel<PartialDailyRestrictionResponse>.Failure(new Error("الحساب غير موجود", Status.NotFound));

            var costCenter = await _unitOfWork.Repository<CostCenterTree>().GetByIdAsync(request.CostCenterId, cancellationToken);

            if (costCenter is null)
                return ErrorResponseModel<PartialDailyRestrictionResponse>.Failure(new Error("مركز التكلفة غير موجود", Status.NotFound));


            var permission = new DispensePermission
            {
                Date = request.Date,
                CostCenterId = request.CostCenterId,
                DispenseTo = request.DispenseTo,
                AccountId = request.AccountId,
                Notes = request.Notes,
                TreasuryId = request.TreasuryId,
                Amount = request.Amount
            };

            await _unitOfWork.Repository<DispensePermission>().AddAsync(permission, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // add transaction to treasury

            var lastMovement = await _unitOfWork.Repository<TreasuryMovement>()
               .GetAll(x => x.TreasuryId == request.TreasuryId && x.IsClosed == false)
               .OrderByDescending(x => x.Id)
               .FirstOrDefaultAsync(cancellationToken);

            var treasuryOperation = new TreasuryOperation
            {
                Date = request.Date,
                Amount = request.Amount,
                Description = request.Notes,
                TreasuryId = request.TreasuryId,
                ReceivedFrom = request.DispenseTo,
                TransactionType = TransactionType.Debit,
                DocumentNumber = permission.Id.ToString(),
                AccountId = request.AccountId,
                TreasuryMovementId = lastMovement?.Id
            };

            lastMovement!.TotalDebits += request.Amount;
            lastMovement.Balance -= request.Amount;

            _unitOfWork.Repository<TreasuryMovement>().Update(lastMovement);
            await _unitOfWork.Repository<TreasuryOperation>().AddAsync(treasuryOperation, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            var dailyRestriction = new DailyRestriction
            {
                RestrictionNumber = await _dailyRestrictionService.GenerateRestrictionNumberAsync(cancellationToken),
                DocumentNumber = treasuryOperation.Id.ToString(),
                RestrictionTypeId = null,
                IsActive = true,
                // TODO : replace it
                AccountingGuidanceId = 17,
                RestrictionDate = request.Date,
                Description = request.Notes,
                Details =
                [
                    new DailyRestrictionDetail
                    {
                        AccountId = request.AccountId,
                        CostCenterId = request.CostCenterId,
                        Credit = 0,
                        Note = null,
                        Debit = request.Amount,
                        From = account.NameAR,
                        To = treasury.Name
                    }
                ]
            };

            await _unitOfWork.Repository<DailyRestriction>().AddAsync(dailyRestriction, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // add daily restriction to dispense permission
            permission.DailyRestrictionId = dailyRestriction.Id;

            _unitOfWork.Repository<DispensePermission>().Update(permission);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            var response = new PartialDailyRestrictionResponse
            {
                Id = permission.Id,
                AccountingGuidanceName = _unitOfWork.Repository<AccountingGuidance>().GetAll(x => x.Id == dailyRestriction.AccountingGuidanceId).FirstOrDefault().Name,
                Amount = request.Amount,
                From = account.NameAR,
                To = treasury.Name,
                RestrictionDate = dailyRestriction.RestrictionDate,
                RestrictionNumber = dailyRestriction.RestrictionNumber,

            };

            return ErrorResponseModel<PartialDailyRestrictionResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return ErrorResponseModel<PartialDailyRestrictionResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var permission = await _unitOfWork.Repository<DispensePermission>()
                .GetByIdAsync(id, cancellationToken);

            if (permission is null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            permission.IsActive = false;

            _unitOfWork.Repository<DispensePermission>().Update(permission);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.GetSuccess);
        }
        catch (Exception)
        {

            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<IReadOnlyList<DispensePermissionResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
    {

        try
        {
            var query = _unitOfWork.Repository<DispensePermission>()
                .GetAll()
                .Include(x => x.Treasury)
                .Include(x => x.CostCenter)
                .Include(x => x.Account)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .OrderByDescending(x => x.Id)
                .Where(x => x.IsActive);


            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                query = query.Where(x => x.Notes.Contains(filter.SearchText) ||
                                         x.Treasury.Name.Contains(filter.SearchText));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var permissions = await query
                .OrderByDescending(x => x.Id)
                .Skip(filter.PageSize * (filter.CurrentPage - 1))
                .Take(filter.PageSize)
                .ToListAsync(cancellationToken);


            var response = permissions.Select(permission => new DispensePermissionResponse
            {
                Id = permission.Id,
                Date = permission.Date,
                AccountId = permission.AccountId,
                AccountNumber = permission?.Account?.AccountNumber ?? string.Empty,
                Amount = permission.Amount,
                CostCenterId = permission.CostCenterId,
                CostCenterNumber = permission?.CostCenter?.CostCenterNumber,
                DispenseTo = permission?.DispenseTo,
                TreasuryId = permission?.TreasuryId,
                TreasuryName = permission?.Treasury?.Name,
                Notes = permission?.Notes,
                Audit = new AuditResponse
                {
                    CreatedBy = permission.CreatedBy?.UserName,
                    CreatedOn = permission.CreatedOn,
                    UpdatedBy = permission.UpdatedBy?.UserName,
                    UpdatedOn = permission.UpdatedOn
                }
            }).ToList();


            return PagedResponseModel<IReadOnlyList<DispensePermissionResponse>>.Success(GenericErrors.GetSuccess, totalCount, response);
        }
        catch (Exception)
        {
            return PagedResponseModel<IReadOnlyList<DispensePermissionResponse>>.Failure(GenericErrors.TransFailed);
        }

    }

    public async Task<ErrorResponseModel<DispensePermissionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var permission = await _unitOfWork.Repository<DispensePermission>()
                .GetAll(x => x.Id == id)
                .Include(x => x.Treasury)
                .Include(x => x.CostCenter)
                .Include(x => x.Account)
                .Include(x => x.DailyRestriction)
                    .ThenInclude(dr => dr.AccountingGuidance)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(cancellationToken);

            if (permission is null)
                return ErrorResponseModel<DispensePermissionResponse>.Failure(GenericErrors.NotFound);

            var response = new DispensePermissionResponse
            {
                Id = permission.Id,
                Date = permission.Date,
                AccountId = permission.AccountId,
                AccountNumber = permission?.Account?.AccountNumber ?? string.Empty,
                Amount = permission.Amount,
                CostCenterId = permission.CostCenterId,
                CostCenterNumber = permission?.CostCenter?.CostCenterNumber,
                DispenseTo = permission?.DispenseTo,
                TreasuryId = permission?.TreasuryId,
                TreasuryName = permission?.Treasury?.Name,
                Notes = permission?.Notes,
                Audit = new AuditResponse
                {
                    CreatedBy = permission.CreatedBy?.UserName,
                    CreatedOn = permission.CreatedOn,
                    UpdatedBy = permission.UpdatedBy?.UserName,
                    UpdatedOn = permission.UpdatedOn
                },
                DailyRestriction = new PartialDailyRestrictionResponse
                {
                    AccountingGuidanceName = permission.DailyRestriction?.AccountingGuidance?.Name ?? string.Empty,
                    Amount = permission.Amount,
                    From = permission.Account?.NameAR ?? string.Empty,
                    To = permission.Treasury?.Name ?? string.Empty,
                    RestrictionDate = permission?.DailyRestriction?.RestrictionDate ?? DateOnly.MinValue,
                    Id = permission?.DailyRestriction?.Id,
                    RestrictionNumber = permission?.DailyRestriction?.RestrictionNumber ?? string.Empty
                }

            };

            return ErrorResponseModel<DispensePermissionResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<DispensePermissionResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, DispensePermissionRequest request, CancellationToken cancellationToken = default)
    {

        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var permission = await _unitOfWork.Repository<DispensePermission>()
                .GetByIdAsync(id, cancellationToken);

            if (permission is null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            var treasury = await _unitOfWork.Repository<Treasury>()
                .GetByIdAsync(request.TreasuryId, cancellationToken);

            if (treasury is null)
                return ErrorResponseModel<string>.Failure(new Error("الخزينه غير موجوده", Status.NotFound));

            var account = await _unitOfWork.Repository<AccountTree>()
                .GetByIdAsync(request.AccountId, cancellationToken);

            if (account is null)
                return ErrorResponseModel<string>.Failure(new Error("الحساب غير موجود", Status.NotFound));

            var costCenter = await _unitOfWork.Repository<CostCenterTree>().GetByIdAsync(request.CostCenterId, cancellationToken);

            if (costCenter is null)
                return ErrorResponseModel<string>.Failure(new Error("مركز التكلفة غير موجود", Status.NotFound));

            permission.Date = request.Date;
            permission.CostCenterId = request.CostCenterId;
            permission.DispenseTo = request.DispenseTo;
            permission.AccountId = request.AccountId;
            permission.Notes = request.Notes;
            permission.TreasuryId = request.TreasuryId;
            permission.Amount = request.Amount;

            _unitOfWork.Repository<DispensePermission>().Update(permission);

            // add transaction to treasury
            var treasuryTransaction = new TreasuryOperation
            {
                Date = request.Date,
                Amount = request.Amount,
                Description = request.Notes,
                TreasuryId = request.TreasuryId,
                ReceivedFrom = treasury.Name,
                TransactionType = TransactionType.Debit,
                DocumentNumber = permission.Id.ToString(),
            };

            await _unitOfWork.Repository<TreasuryOperation>().AddAsync(treasuryTransaction, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.GetSuccess);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }
}