using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.DebitNotices;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS;
public class DebitNoticeService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IDebitNoticeService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISQLHelper _sQLHelper = sQLHelper;

    public async Task<ErrorResponseModel<string>> CreateAsync(DebitNoticeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var debitNotice = new DebitNotice
            {
                AccountId = request.AccountId,
                Amount = request.Amount,
                BankId = request.BankId,
                CheckNumber = request.CheckNumber,
                Date = request.Date,
                Notes = request.Notes,

            };

            await _unitOfWork.Repository<DebitNotice>().AddAsync(debitNotice, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, debitNotice.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var debitNotice = _unitOfWork.Repository<DebitNotice>()
                .GetAll(x => x.Id == id)
                .FirstOrDefault();

            if (debitNotice == null)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }

            debitNotice.IsActive = false;

            _unitOfWork.Repository<DebitNotice>().Update(debitNotice);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, debitNotice.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
    {
        try
        {
            var parameters = new List<SqlParameter>
            {
                new("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value),
                new("@CurrentPage", pagingFilter.CurrentPage),
                new("@PageSize", pagingFilter.PageSize)
            };

            if (pagingFilter.FilterList != null)
            {
                var bankFilter = pagingFilter.FilterList.FirstOrDefault(f => f.CategoryName == "Bank");
                if (bankFilter != null)
                {
                    parameters.Add(new SqlParameter("@BankId", bankFilter.ItemValue ?? (object)DBNull.Value));
                }

                var accountFilter = pagingFilter.FilterList.FirstOrDefault(f => f.CategoryName == "Account");
                if (accountFilter != null)
                {
                    parameters.Add(new SqlParameter("@AccountId", accountFilter.ItemValue ?? (object)DBNull.Value));
                }

                var dateFilter = pagingFilter.FilterList.FirstOrDefault(f => f.CategoryName == "Date");
                if (dateFilter != null)
                {
                    parameters.Add(new SqlParameter("@FromDate", dateFilter.FromDate ?? (object)DBNull.Value));
                    parameters.Add(new SqlParameter("@ToDate", dateFilter.ToDate ?? (object)DBNull.Value));
                }

                var checkNumberFilter = pagingFilter.FilterList.FirstOrDefault(f => f.CategoryName == "CheckNumber");
                if (checkNumberFilter != null)
                {
                    parameters.Add(new SqlParameter("@CheckNumber", checkNumberFilter.ItemValue ?? (object)DBNull.Value));
                }
            }

            var dt = await _sQLHelper.ExecuteDataTableAsync("[finance].[SP_GetDebitNotices]", parameters.ToArray());

            int totalCount = 0;
            if (dt.Rows.Count > 0)
            {
                totalCount = dt.Rows[0].Field<int>("TotalCount");
            }

            return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, totalCount, dt);
        }
        catch (Exception)
        {
            return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
        }
    }
    public async Task<ErrorResponseModel<DebitNoticeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var debitNotice = await _unitOfWork.Repository<DebitNotice>()
                .GetAll(x => x.Id == id && x.IsActive)
                .Include(x => x.CreatedBy)
                .Include(x => x.Bank)
                .Include(x => x.Account)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync();

            if (debitNotice == null)
            {
                return ErrorResponseModel<DebitNoticeResponse>.Failure(GenericErrors.NotFound);
            }

            var response = new DebitNoticeResponse
            {
                Id = debitNotice.Id,
                AccountId = debitNotice.AccountId,
                Amount = debitNotice.Amount,
                BankId = debitNotice.BankId,
                CheckNumber = debitNotice.CheckNumber,
                Date = debitNotice.Date,
                Notes = debitNotice.Notes,
                AccountName = debitNotice.Account.Name,
                BankName = debitNotice.Bank.Name,
                Audit = new Core.Contracts.Common.AuditResponse
                {
                    CreatedBy = debitNotice.CreatedBy.UserName,
                    CreatedOn = debitNotice.CreatedOn,
                    UpdatedBy = debitNotice.UpdatedBy?.UserName,
                    UpdatedOn = debitNotice.UpdatedOn
                }
            };

            return ErrorResponseModel<DebitNoticeResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<DebitNoticeResponse>.Failure(GenericErrors.TransFailed);
        }

    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, DebitNoticeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var debitNotice = await _unitOfWork.Repository<DebitNotice>()
                .GetAll(x => x.Id == id && x.IsActive)
                .FirstOrDefaultAsync(cancellationToken);

            if (debitNotice == null)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }

            debitNotice.AccountId = request.AccountId;
            debitNotice.Amount = request.Amount;
            debitNotice.BankId = request.BankId;
            debitNotice.CheckNumber = request.CheckNumber;
            debitNotice.Date = request.Date;
            debitNotice.Notes = request.Notes;

            _unitOfWork.Repository<DebitNotice>().Update(debitNotice);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, debitNotice.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }
}
