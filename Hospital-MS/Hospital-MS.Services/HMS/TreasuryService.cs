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
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                .AnyAsync(x => x.AccountCode == request.AccountCode ||
                              (x.Name == request.Name && x.BranchId == request.BranchId),
                         cancellationToken);

            if (existingTreasury)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            var treasury = new Treasury
            {
                AccountCode = request.AccountCode,
                Name = request.Name,
                BranchId = request.BranchId,
                Currency = request.Currency,
                OpeningBalance = request.OpeningBalance,
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
                AccountCode = row.Field<string>("AccountCode"),
                Name = row.Field<string>("Name"),
                BranchId = row.Field<int>("BranchId"),
                BranchName = row.Field<string>("BranchName"),
                Currency = row.Field<string>("Currency"),
                OpeningBalance = row.Field<decimal>("OpeningBalance"),
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
                AccountCode = treasury.AccountCode,
                Name = treasury.Name,
                BranchId = treasury.BranchId,
                BranchName = treasury.Branch.Name,
                Currency = treasury.Currency,
                OpeningBalance = treasury.OpeningBalance,
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
                    (x.AccountCode == request.AccountCode ||
                    (x.Name == request.Name && x.BranchId == request.BranchId)),
                    cancellationToken);

            if (isExist)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            treasury.AccountCode = request.AccountCode;
            treasury.Name = request.Name;
            treasury.BranchId = request.BranchId;
            treasury.Currency = request.Currency;
            treasury.OpeningBalance = request.OpeningBalance;

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

    public async Task<ErrorResponseModel<List<TreasuryResponse>>> GetEnabledTreasuriesAsync(CancellationToken cancellationToken = default)
    {
        var treasuries = await _unitOfWork.Repository<Treasury>()
            .GetAll(x => x.IsActive && x.IsEnabled)
            .Select(x => new TreasuryResponse
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToListAsync(cancellationToken);

        return ErrorResponseModel<List<TreasuryResponse>>.Success(GenericErrors.GetSuccess, treasuries);
    }

    public async Task<ErrorResponseModel<List<TreasuryResponse>>> GetDisabledTreasuriesAsync(CancellationToken cancellationToken = default)
    {
        var treasuries = await _unitOfWork.Repository<Treasury>()
            .GetAll(x => x.IsActive && !x.IsEnabled)
            .Select(x => new TreasuryResponse
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToListAsync(cancellationToken);

        return ErrorResponseModel<List<TreasuryResponse>>.Success(GenericErrors.GetSuccess, treasuries);
    }

    public async Task<ErrorResponseModel<string>> EnableTreasuryAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var treasury = await _unitOfWork.Repository<Treasury>().GetByIdAsync(id, cancellationToken);

            if (treasury == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            treasury.IsEnabled = true;

            _unitOfWork.Repository<Treasury>().Update(treasury);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> DisableTreasuryAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var treasury = await _unitOfWork.Repository<Treasury>().GetByIdAsync(id, cancellationToken);

            if (treasury == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            treasury.IsEnabled = false;

            _unitOfWork.Repository<Treasury>().Update(treasury);

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
            var transactionsQuery = _unitOfWork.Repository<TreasuryTransaction>()
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
                    Debit = t.TransactionType == TransactionType.Debit ? t.Amount : 0
                }).ToList()
            };

            return ErrorResponseModel<TreasuryTransactionResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<TreasuryTransactionResponse>.Failure(GenericErrors.TransFailed);
        }
    }
}