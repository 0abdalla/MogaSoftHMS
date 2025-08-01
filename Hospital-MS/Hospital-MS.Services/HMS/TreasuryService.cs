﻿using Hospital_MS.Core.Common;
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

            // Get all treasury IDs from the result
            var treasuryIds = dt.AsEnumerable().Select(row => row.Field<int>("Id")).ToList();

            // Query movements for these treasuries

            // i need to return bool if the movement is has operations or not (use treasury operations table)


            var movements = await _unitOfWork.Repository<TreasuryMovement>()
                .GetAll(x => treasuryIds.Contains(x.TreasuryId) && x.IsActive)
                .Select(x => new
                {
                    x.TreasuryId,
                    x.Id,
                    x.OpenedIn,
                    x.ClosedIn,
                    x.IsClosed,
                    x.TreasuryNumber,
                    x.IsReEnabled
                })
                .ToListAsync(cancellationToken);

            var treasuries = dt.AsEnumerable().Select(row =>
            {
                var id = row.Field<int>("Id");
                var movementList = movements
                    .Where(m => m.TreasuryId == id)
                    .Select(m => new PartialMovementResponse
                    {
                        MovementId = m.Id,
                        OpenedIn = m.OpenedIn,
                        ClosedIn = m.ClosedIn,
                        IsClosed = m.IsClosed,
                        TreasuryNumber = m.TreasuryNumber,
                        IsReEnabled = m.IsReEnabled
                    })
                    .ToList();

                return new TreasuryResponse
                {
                    Id = id,
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
                    },
                    Movements = movementList
                };
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
                OpenedIn = x.OpenedIn,
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
            .GetAll(x => x.IsActive && x.IsClosed)
            .Include(x => x.Treasury)
            .Select(x => new TreasuryMovementResponse
            {
                Id = x.Id,
                ClosedIn = x.ClosedIn,
                OpenedIn = x.OpenedIn,
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
            movement.IsReEnabled = true;

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

    public async Task<ErrorResponseModel<string>> DisableTreasuryMovementAsync(int id, DateOnly closeInDate, CancellationToken cancellationToken = default)
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
                t.TransactionType == TransactionType.Credit ? t.Amount : -t.Amount);

            //var balance = operations.Sum(t =>
            //    t.TransactionType == Core.Enums.TransactionType.Credit ? t.Amount : -t.Amount);

            treasuryMovement.IsClosed = true;
            treasuryMovement.ClosedIn = closeInDate;

            // Get the latest TreasuryNumber for this TreasuryId
            var lastTreasuryNumber = await _unitOfWork.Repository<TreasuryMovement>()
                .GetAll(x => x.TreasuryId == treasury.Id)
                .OrderByDescending(x => x.TreasuryNumber)
                .Select(x => x.TreasuryNumber)
                .FirstOrDefaultAsync(cancellationToken);

            var nextTreasuryNumber = lastTreasuryNumber + 1;

            var newMovement = new TreasuryMovement
            {
                TreasuryId = treasury.Id,
                OpenedIn = closeInDate.AddDays(1),
                ClosedIn = closeInDate.AddDays(1),
                OpeningBalance = newBalance,
                TreasuryNumber = nextTreasuryNumber,
                TotalCredits = operations
                    .Where(t => t.TransactionType == Core.Enums.TransactionType.Credit)
                    .Sum(t => t.Amount),
                TotalDebits = operations
                    .Where(t => t.TransactionType == Core.Enums.TransactionType.Debit)
                    .Sum(t => t.Amount),
                // Balance = balance,
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
            OpenedIn = mov.OpenedIn,
            TreasuryId = mov.TreasuryId,
            TreasuryName = mov.Treasury.Name,
            IsClosed = mov.IsClosed,

        }).ToListAsync(cancellationToken);

        return PagedResponseModel<List<TreasuryMovementResponse>>.Success(GenericErrors.GetSuccess, totalCount, response);
    }
    public async Task<ErrorResponseModel<TreasuryMovementDetailsResponse>> GetTreasuryMovementByIdAsyncV2(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var movement = await _unitOfWork.Repository<TreasuryMovement>()
                .GetAll()
                .Include(x => x.Treasury)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (movement == null)
                return ErrorResponseModel<TreasuryMovementDetailsResponse>.Failure(GenericErrors.NotFound);

            var fromDate = movement.OpenedIn;
            var toDate = movement.ClosedIn ?? movement.OpenedIn;

            // Get all operations for this movement
            var operations = await _unitOfWork.Repository<TreasuryOperation>()
                .GetAll(t => t.TreasuryId == movement.TreasuryId && t.IsActive)
                .Where(t => t.Date >= fromDate && t.Date <= toDate)
                .OrderBy(t => t.Date)
                .ThenBy(t => t.DocumentNumber)
                //.Include(t => t.Account)
                .ToListAsync(cancellationToken);

            // Previous balance before movement
            var previousBalance = await _unitOfWork.Repository<TreasuryOperation>()
                .GetAll(t => t.TreasuryId == movement.TreasuryId && t.IsActive && t.Date < fromDate)
                .SumAsync(t => t.TransactionType == TransactionType.Credit ? t.Amount : -t.Amount, cancellationToken);

            // Receipts (Credits)
            var receipts = operations
                .Where(t => t.TransactionType == TransactionType.Credit)
                .Select(t => new TreasuryTransactionRow
                {
                    DocumentNumber = t.Id,
                    Date = t.Date,
                    // AccountName = t.Account != null ? t.Account.Name : "",
                    Description = t.Description ?? "",
                    Value = t.Amount
                }).ToList();

            // Payments (Debits)
            var payments = operations
                .Where(t => t.TransactionType == TransactionType.Debit)
                .Select(t => new TreasuryTransactionRow
                {
                    DocumentNumber = t.Id,
                    Date = t.Date,
                    //AccountName = t.Account != null ? t.Account.Name : "",
                    Description = t.Description ?? "",
                    Value = t.Amount
                }).ToList();

            var totalReceipts = receipts.Sum(x => x.Value);
            var totalPayments = payments.Sum(x => x.Value);
            var closingBalance = previousBalance + totalReceipts - totalPayments;

            var response = new TreasuryMovementDetailsResponse
            {
                MovementId = movement.Id,
                TreasuryName = movement.Treasury.Name,
                FromDate = fromDate,
                ToDate = toDate,
                PreviousBalance = previousBalance,
                TotalReceipts = totalReceipts,
                TotalPayments = totalPayments,
                ClosingBalance = closingBalance,
                Receipts = receipts,
                Payments = payments
            };

            return ErrorResponseModel<TreasuryMovementDetailsResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<TreasuryMovementDetailsResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<TreasuryMovementResponse>> GetTreasuryMovementByIdAsyncV1(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var movement = await _unitOfWork.Repository<TreasuryMovement>()
                .GetAll()
                .Include(x => x.Treasury)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (movement == null)
                return ErrorResponseModel<TreasuryMovementResponse>.Failure(GenericErrors.NotFound);

            // Get all operations for this treasury movement
            //var operations = await _unitOfWork.Repository<TreasuryOperation>()
            //    .GetAll(t => t.TreasuryId == movement.TreasuryId && t.IsActive)
            //    .Where(t => t.Date >= movement.OpenedIn &&
            //               (!movement.ClosedIn.HasValue || t.Date <= movement.ClosedIn.Value))
            //    .OrderBy(t => t.Date)
            //    .ThenBy(t => t.DocumentNumber)
            //    .ToListAsync(cancellationToken);

            var operations = await _unitOfWork.Repository<TreasuryOperation>()
                .GetAll(x => x.TreasuryMovementId == id)
                .ToListAsync(cancellationToken);

            var fromDate = movement.OpenedIn;
            var toDate = movement.ClosedIn ?? movement.OpenedIn;

            //// Previous balance before movement
            //var previousBalance = await _unitOfWork.Repository<TreasuryOperation>()
            //    .GetAll(t => t.TreasuryId == movement.TreasuryId && t.IsActive && t.Date < fromDate)
            //    .SumAsync(t => t.TransactionType == TransactionType.Credit ? t.Amount : -t.Amount, cancellationToken);

            // pervious balance will be the balance of pervious movement 
            var previousBalance = await _unitOfWork.Repository<TreasuryMovement>()
                .GetAll(t => t.TreasuryId == movement.TreasuryId && t.IsActive && t.Id < movement.Id)
                .OrderByDescending(t => t.Id)
                .Select(t => t.Balance)
                .FirstOrDefaultAsync(cancellationToken);

            // Receipts (Credits)
            var receipts = operations
                .Where(t => t.TransactionType == TransactionType.Credit)
                .Select(t => new TreasuryTransactionRow
                {
                    Value = t.Amount

                }).ToList();

            // Payments (Debits)
            var payments = operations
                .Where(t => t.TransactionType == TransactionType.Debit)
                .Select(t => new TreasuryTransactionRow
                {
                    Value = t.Amount

                }).ToList();

            var totalReceipts = receipts.Sum(x => x.Value);
            var totalPayments = payments.Sum(x => x.Value);
            var closingBalance = previousBalance + totalReceipts - totalPayments;

            var response = new TreasuryMovementResponse
            {
                Id = movement.Id,
                TreasuryId = movement.TreasuryId,
                TreasuryName = movement.Treasury.Name,
                TreasuryNumber = movement.TreasuryNumber,
                OpeningBalance = movement.OpeningBalance,
                OpenedIn = movement.OpenedIn,
                ClosedIn = movement.ClosedIn,
                IsClosed = movement.IsClosed,
                TotalCredits = totalReceipts,
                TotalDebits = totalPayments,
                Balance = movement.Balance,
                ClosingBalance = closingBalance,
                PreviousBalance = previousBalance,
                Transactions = operations.Select(t => new TransactionDetail
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

            return ErrorResponseModel<TreasuryMovementResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<TreasuryMovementResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> ReDisableTreasuryMovementAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var movement = await _unitOfWork.Repository<TreasuryMovement>()
                .GetByIdAsync(id, cancellationToken);

            if (movement == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            movement.IsClosed = true;

            _unitOfWork.Repository<TreasuryMovement>().Update(movement);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }
}