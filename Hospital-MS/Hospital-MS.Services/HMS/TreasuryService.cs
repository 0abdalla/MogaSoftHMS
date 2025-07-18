using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.Treasuries;
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
public class TreasuryService : ITreasuryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISQLHelper _sqlHelper;

    public TreasuryService(IUnitOfWork unitOfWork, ISQLHelper sqlHelper)
    {
        _unitOfWork = unitOfWork;
        _sqlHelper = sqlHelper;
    }

    public async Task<ErrorResponseModel<string>> AssignTreasuryToStaffAsync(int staffId, int treasuryId, CancellationToken cancellationToken)
    {
        try
        {
            var staffTreasury = new StaffTreasury
            {
                StaffId = staffId,
                TreasuryId = treasuryId
            };

            await _unitOfWork.Repository<StaffTreasury>().AddAsync(staffTreasury, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.GetSuccess);
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> CreateTreasuryAsync(TreasuryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var existingTreasury = await _unitOfWork.Repository<Treasury>()
                .AnyAsync(x => x.Code == request.Code ||
                              (x.Name == request.Name && x.BranchId == request.BranchId),
                         cancellationToken);

            if (existingTreasury)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            var treasury = new Treasury
            {
                Code = request.Code,
                Name = request.Name,
                BranchId = request.BranchId,
                Currency = request.Currency,
                IsActive = true
            };

            await _unitOfWork.Repository<Treasury>().AddAsync(treasury, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, treasury.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<TreasuryResponse>>> GetTreasuriesAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken)
    {
        try
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value),
                new SqlParameter("@CurrentPage", pagingFilter.CurrentPage),
                new SqlParameter("@PageSize", pagingFilter.PageSize)
            };

            var dt = await _sqlHelper.ExecuteDataTableAsync("Finance.SP_GetAllTreasuries", parameters);

            var treasuries = dt.AsEnumerable().Select(row => new TreasuryResponse
            {
                Id = row.Field<int>("Id"),
                Code = row.Field<string>("Code"),
                Name = row.Field<string>("Name"),
                BranchId = row.Field<int>("BranchId"),
                BranchName = row.Field<string>("BranchName"),
                Currency = row.Field<string>("Currency"),
                IsActive = row.Field<bool>("IsActive"),
                Audit = new AuditResponse
                {
                    CreatedBy = row.Field<string>("CreatedBy"),
                    CreatedOn = row.Field<DateTime>("CreatedOn")
                }
            }).ToList();

            int totalCount = dt.Rows.Count > 0 ? dt.Rows[0].Field<int>("TotalCount") : 0;

            return PagedResponseModel<List<TreasuryResponse>>.Success(GenericErrors.GetSuccess, totalCount, treasuries);
        }
        catch (Exception)
        {
            return PagedResponseModel<List<TreasuryResponse>>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<TreasuryResponse>> GetTreasuryByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var treasury = await _unitOfWork.Repository<Treasury>()
                .GetAll()
                .Include(x => x.Branch)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (treasury == null)
                return ErrorResponseModel<TreasuryResponse>.Failure(GenericErrors.NotFound);

            var response = new TreasuryResponse
            {
                Id = treasury.Id,
                Code = treasury.Code,
                Name = treasury.Name,
                BranchId = treasury.BranchId,
                BranchName = treasury.Branch.Name,
                Currency = treasury.Currency,
                IsActive = treasury.IsActive,
                Audit = new AuditResponse
                {
                    CreatedBy = treasury.CreatedBy.UserName,
                    CreatedOn = treasury.CreatedOn,
                    UpdatedBy = treasury.UpdatedBy?.UserName,
                    UpdatedOn = treasury.UpdatedOn
                }
            };

            return ErrorResponseModel<TreasuryResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<TreasuryResponse>.Failure(GenericErrors.TransFailed);
        }

    }

    public async Task<ErrorResponseModel<string>> UpdateTreasuryAsync(int id, TreasuryRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var treasury = await _unitOfWork.Repository<Treasury>().GetByIdAsync(id, cancellationToken);

            if (treasury == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            var isExist = await _unitOfWork.Repository<Treasury>()
                .AnyAsync(x => x.Id != id &&
                    (x.Code == request.Code ||
                    (x.Name == request.Name && x.BranchId == request.BranchId)),
                    cancellationToken);

            if (isExist)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            treasury.Code = request.Code;
            treasury.Name = request.Name;
            treasury.BranchId = request.BranchId;
            treasury.Currency = request.Currency;


            _unitOfWork.Repository<Treasury>().Update(treasury);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> DeleteTreasuryAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var treasury = await _unitOfWork.Repository<Treasury>().GetByIdAsync(id, cancellationToken);

            if (treasury == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            treasury.IsActive = false;

            _unitOfWork.Repository<Treasury>().Update(treasury);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<List<TreasuryMovementResponse>>> GetEnabledTreasuriesMovementsAsync(CancellationToken cancellationToken = default)
    {

        var treasuries = await _unitOfWork.Repository<TreasuryMovement>()
            .GetAll(x => x.IsActive && !x.IsClosed)
            .Include(x => x.Treasury)
            .Select(x => new TreasuryMovementResponse
            {
                Id = x.Id,
                ClosedIn = x.ClosedIn,
                OpenedId = x.OpenedIn,
                TreasuryId = x.TreasuryId,
                TreasuryName = x.Treasury.Name,
                IsClosed = x.IsClosed
            })
            .ToListAsync(cancellationToken);

        return ErrorResponseModel<List<TreasuryMovementResponse>>.Success(GenericErrors.GetSuccess, treasuries);
    }

    public async Task<ErrorResponseModel<List<TreasuryMovementResponse>>> GetDisabledTreasuriesMovementsAsync(CancellationToken cancellationToken = default)
    {
        var treasuries = await _unitOfWork.Repository<TreasuryMovement>()
            .GetAll(x => x.IsActive && !x.IsClosed)
            .Include(x => x.Treasury)
            .Select(x => new TreasuryMovementResponse
            {
                Id = x.Id,
                ClosedIn = x.ClosedIn,
                OpenedId = x.OpenedIn,
                TreasuryId = x.TreasuryId,
                TreasuryName = x.Treasury.Name,
                IsClosed = x.IsClosed
            })
            .ToListAsync(cancellationToken);

        return ErrorResponseModel<List<TreasuryMovementResponse>>.Success(GenericErrors.GetSuccess, treasuries);
    }

    public async Task<ErrorResponseModel<string>> EnableTreasuryMovementAsync(int treasuryMovementId, CancellationToken cancellationToken = default)
    {
        try
        {
            var movement = await _unitOfWork.Repository<TreasuryMovement>()
                .GetByIdAsync(treasuryMovementId, cancellationToken);

            if (movement == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            movement.IsClosed = false;

            _unitOfWork.Repository<TreasuryMovement>().Update(movement);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<TreasuryTransactionResponse>> GetTreasuryTransactionsAsync(int treasuryId, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default)
    {
        try
        {
            var transactionsQuery = _unitOfWork.Repository<TreasuryOperation>()
                .GetAll()
                .Where(t => t.TreasuryId == treasuryId && t.IsActive);


            var previousBalance = await transactionsQuery
                .Where(t => t.Date < fromDate)
                .SumAsync(t => t.TransactionType == TransactionType.Credit ? t.Amount : -t.Amount, cancellationToken);

            var periodTransactions = await transactionsQuery
                .Where(t => t.Date >= fromDate && t.Date <= toDate)
                .OrderBy(t => t.Date)
                .ThenBy(t => t.DocumentNumber)
                .ToListAsync(cancellationToken);

            if (!periodTransactions.Any())
                return ErrorResponseModel<TreasuryTransactionResponse>.Failure(GenericErrors.NotFound);

            var totalCredits = periodTransactions
                .Where(t => t.TransactionType == TransactionType.Credit)
                .Sum(t => t.Amount);

            var totalDebits = periodTransactions
                .Where(t => t.TransactionType == TransactionType.Debit)
                .Sum(t => t.Amount);

            var currentBalance = previousBalance + (totalCredits - totalDebits);

            var response = new TreasuryTransactionResponse
            {
                TreasuryId = treasuryId,
                FromDate = fromDate,
                ToDate = toDate,
                PreviousBalance = previousBalance,
                TotalCredits = totalCredits,
                TotalDebits = totalDebits,
                CurrentBalance = currentBalance,
                Transactions = periodTransactions.Select(t => new TransactionDetail
                {
                    DocumentId = t.DocumentNumber,
                    Date = t.Date.ToDateTime(TimeOnly.MinValue),
                    Description = t.Description ?? string.Empty,
                    ReceivedFrom = t.ReceivedFrom ?? string.Empty,
                    Credit = t.TransactionType == TransactionType.Credit ? t.Amount : 0,
                    Debit = t.TransactionType == TransactionType.Debit ? t.Amount : 0,
                    AccountId = t.AccountId,
                }).ToList()
            };

            return ErrorResponseModel<TreasuryTransactionResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<TreasuryTransactionResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> DisableTreasuryMovementAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var treasury = await _unitOfWork.Repository<Treasury>()
                .GetAll()
                .Include(x => x.Branch)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (treasury == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound, "الخزينه غير موجوده");

            var treasuryMovement = await _unitOfWork.Repository<TreasuryMovement>()
                .GetAll(x => x.TreasuryId == id && x.IsActive && !x.IsClosed)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);


            if (treasuryMovement == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound, "لا يوجد حركه مفتوحه لهذه الخزينه");

            // حساب الرصيد الحالي
            var operations = await _unitOfWork.Repository<TreasuryOperation>()
                .GetAll(t => t.TreasuryId == id && t.IsActive)
                .ToListAsync(cancellationToken);

            var newBalance = treasuryMovement.OpeningBalance + operations.Sum(t =>
                t.TransactionType == Core.Enums.TransactionType.Credit ? t.Amount : -t.Amount);

            var balance = operations.Sum(t =>
                t.TransactionType == Core.Enums.TransactionType.Credit ? t.Amount : -t.Amount);

            treasuryMovement.IsClosed = true;
            treasuryMovement.ClosedIn = DateOnly.FromDateTime(DateTime.Today);

            var newMovement = new TreasuryMovement
            {
                TreasuryId = treasury.Id,
                OpenedIn = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                OpeningBalance = newBalance,
                TreasuryNumber = treasuryMovement.TreasuryNumber + 1,
                TotalCredits = operations
                    .Where(t => t.TransactionType == TransactionType.Credit)
                    .Sum(t => t.Amount),
                TotalDebits = operations
                    .Where(t => t.TransactionType == TransactionType.Debit)
                    .Sum(t => t.Amount),
                Balance = balance,
            };

            _unitOfWork.Repository<TreasuryMovement>().Update(treasuryMovement);
            await _unitOfWork.Repository<TreasuryMovement>().AddAsync(newMovement, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<TreasuryMovementResponse>>> GetAllMovementsAsync(CancellationToken cancellationToken = default)
    {

        var movements = _unitOfWork.Repository<TreasuryMovement>()
            .GetAll(x => x.IsActive)
            .Include(x => x.Treasury)
            .AsQueryable();

        var totalCount = await movements.CountAsync(cancellationToken: cancellationToken);

        var response = await movements.Select(mov => new TreasuryMovementResponse
        {
            Id = mov.Id,
            ClosedIn = mov.ClosedIn,
            OpenedId = mov.OpenedIn,
            TreasuryId = mov.TreasuryId,
            TreasuryName = mov.Treasury.Name,
            IsClosed = mov.IsClosed,
            
        }).ToListAsync(cancellationToken);

        return PagedResponseModel<List<TreasuryMovementResponse>>.Success(GenericErrors.GetSuccess, totalCount, response);
    }
}