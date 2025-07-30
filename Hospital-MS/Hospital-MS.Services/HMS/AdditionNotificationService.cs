using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.AdditionNotifications;
using Hospital_MS.Core.Contracts.DailyRestrictions;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Hospital_MS.Services.HMS;
public class AdditionNotificationService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper, IDailyRestrictionService dailyRestrictionService) : IAdditionNotificationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISQLHelper _sQLHelper = sQLHelper;
    private readonly IDailyRestrictionService _dailyRestrictionService = dailyRestrictionService;

    public async Task<ErrorResponseModel<PartialDailyRestrictionResponse>> CreateAsync(AdditionNotificationRequest request, CancellationToken cancellationToken = default)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var account = await _unitOfWork.Repository<AccountTree>()
                .GetAll(x => x.AccountId == request.AccountId)
                .FirstOrDefaultAsync(cancellationToken);

            if (account == null)
                return ErrorResponseModel<PartialDailyRestrictionResponse>.Failure(GenericErrors.NotFound);

            var bank = await _unitOfWork.Repository<Bank>()
                .GetAll(x => x.Id == request.BankId && x.IsActive)
                .FirstOrDefaultAsync(cancellationToken);

            if (bank == null)
                return ErrorResponseModel<PartialDailyRestrictionResponse>.Failure(GenericErrors.NotFound);


            var notification = new AdditionNotice
            {
                Date = request.Date,
                BankId = request.BankId,
                AccountId = request.AccountId,
                CheckNumber = request.CheckNumber,
                Amount = request.Amount,
                Notes = request.Notes
            };

            await _unitOfWork.Repository<AdditionNotice>().AddAsync(notification, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            var dailyRestriction = new DailyRestriction
            {
                RestrictionNumber = await _dailyRestrictionService.GenerateRestrictionNumberAsync(cancellationToken),
                DocumentNumber = notification.Id.ToString(),
                RestrictionTypeId = null,
                IsActive = true,
                AccountingGuidanceId = 2,
                RestrictionDate = request.Date,
                Description = request.Notes,
                Details =
                [
                    new DailyRestrictionDetail
                    {
                        AccountId = request.AccountId,
                        CostCenterId = null,
                        Credit = request.Amount,
                        Note = null,
                        Debit = 0,
                    }
                ]
            };

            await _unitOfWork.Repository<DailyRestriction>().AddAsync(dailyRestriction, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            var response = new PartialDailyRestrictionResponse
            {
                Id = notification.Id,
                AccountingGuidanceName = _unitOfWork.Repository<AccountingGuidance>().GetAll(x => x.Id == dailyRestriction.AccountingGuidanceId).FirstOrDefault().Name,
                Amount = request.Amount,
                From = bank.Name,
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

    public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var notification = await _unitOfWork.Repository<AdditionNotice>()
                .GetAll(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (notification == null)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }

            notification.IsActive = false;

            _unitOfWork.Repository<AdditionNotice>().Update(notification);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }

    }

    public async Task<PagedResponseModel<List<AdditionNotificationResponse>>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<AdditionNotice>()
                .GetAll(x => x.IsActive)
                .Include(x => x.Bank)
                .Include(x => x.Account)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagingFilter.SearchText))
            {
                var search = pagingFilter.SearchText.Trim();
                query = query.Where(x =>
                    x.CheckNumber.Contains(search) ||
                    x.Bank.Name.Contains(search) ||
                    x.Account.NameAR.Contains(search) ||
                    x.Account.NameEN.Contains(search) ||
                    x.Notes.Contains(search));
            }

            if (pagingFilter.FilterList != null)
            {
                var bankFilter = pagingFilter.FilterList.FirstOrDefault(f => f.CategoryName == "Bank");
                if (bankFilter != null && int.TryParse(bankFilter.ItemValue, out var bankId))
                    query = query.Where(x => x.BankId == bankId);

                var accountFilter = pagingFilter.FilterList.FirstOrDefault(f => f.CategoryName == "Account");
                if (accountFilter != null && int.TryParse(accountFilter.ItemValue, out var accountId))
                    query = query.Where(x => x.AccountId == accountId);

                var dateFilter = pagingFilter.FilterList.FirstOrDefault(f => f.CategoryName == "Date");
                if (dateFilter != null)
                {
                    if (dateFilter.FromDate.HasValue)
                        query = query.Where(x => x.Date >= DateOnly.FromDateTime(dateFilter.FromDate.Value));
                    if (dateFilter.ToDate.HasValue)
                        query = query.Where(x => x.Date <= DateOnly.FromDateTime(dateFilter.ToDate.Value));
                }

                var checkNumberFilter = pagingFilter.FilterList.FirstOrDefault(f => f.CategoryName == "CheckNumber");
                if (checkNumberFilter != null && !string.IsNullOrWhiteSpace(checkNumberFilter.ItemValue))
                    query = query.Where(x => x.CheckNumber.Contains(checkNumberFilter.ItemValue));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var notifications = await query
                .OrderByDescending(x => x.Date)
                .Skip((pagingFilter.CurrentPage - 1) * pagingFilter.PageSize)
                .Take(pagingFilter.PageSize)
                .Select(x => new AdditionNotificationResponse
                {
                    Id = x.Id,
                    Date = x.Date,
                    BankId = x.BankId,
                    BankName = x.Bank.Name,
                    AccountId = x.AccountId,
                    AccountName = x.Account.NameAR ?? x.Account.NameEN ?? "",
                    CheckNumber = x.CheckNumber,
                    Amount = x.Amount,
                    Notes = x.Notes,
                    Audit = new Core.Contracts.Common.AuditResponse
                    {
                        CreatedBy = x.CreatedBy != null ? x.CreatedBy.UserName : "",
                        CreatedOn = x.CreatedOn,
                        UpdatedBy = x.UpdatedBy != null ? x.UpdatedBy.UserName : null,
                        UpdatedOn = x.UpdatedOn
                    }
                })
                .ToListAsync(cancellationToken);

            return PagedResponseModel<List<AdditionNotificationResponse>>.Success(
                GenericErrors.GetSuccess,
                totalCount,
                notifications
            );
        }
        catch (Exception)
        {
            return PagedResponseModel<List<AdditionNotificationResponse>>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<AdditionNotificationResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var notification = await _unitOfWork.Repository<AdditionNotice>()
                .GetAll(x => x.Id == id && x.IsActive)
                .Include(x => x.Bank)
                .Include(x => x.Account)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(cancellationToken);
            if (notification == null)
            {
                return ErrorResponseModel<AdditionNotificationResponse>.Failure(GenericErrors.NotFound);
            }
            var response = new AdditionNotificationResponse
            {
                Id = notification.Id,
                Date = notification.Date,
                BankId = notification.BankId,
                BankName = notification.Bank.Name,
                AccountId = notification.AccountId,
                AccountName = notification.Account.NameAR ?? notification.Account.NameEN ?? "",
                CheckNumber = notification.CheckNumber,
                Amount = notification.Amount,
                Notes = notification.Notes,
                Audit = new Core.Contracts.Common.AuditResponse
                {
                    CreatedBy = notification.CreatedBy.UserName,
                    CreatedOn = notification.CreatedOn,
                    UpdatedBy = notification.UpdatedBy?.UserName,
                    UpdatedOn = notification.UpdatedOn
                }
            };
            return ErrorResponseModel<AdditionNotificationResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<AdditionNotificationResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, AdditionNotificationRequest request, CancellationToken cancellationToken = default)
    {

        try
        {
            var notification = await _unitOfWork.Repository<AdditionNotice>()
                .GetAll(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (notification == null)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }

            notification.Date = request.Date;
            notification.BankId = request.BankId;
            notification.AccountId = request.AccountId;
            notification.CheckNumber = request.CheckNumber;
            notification.Amount = request.Amount;
            notification.Notes = request.Notes;

            _unitOfWork.Repository<AdditionNotice>().Update(notification);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);

        }
    }
}