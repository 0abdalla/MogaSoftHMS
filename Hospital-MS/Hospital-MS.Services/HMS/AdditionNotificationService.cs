using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.AdditionNotifications;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS;
public class AdditionNotificationService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IAdditionNotificationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISQLHelper _sQLHelper = sQLHelper;

    public async Task<ErrorResponseModel<string>> CreateAsync(AdditionNotificationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
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

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, notification.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var notification = _unitOfWork.Repository<AdditionNotice>()
                .GetAll(x => x.Id == id)
                .FirstOrDefault();

            if (notification == null)
            {
                return Task.FromResult(ErrorResponseModel<string>.Failure(GenericErrors.NotFound));
            }

            notification.IsActive = false;

            _unitOfWork.Repository<AdditionNotice>().Update(notification);
            _unitOfWork.CompleteAsync(cancellationToken);
            return Task.FromResult(ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, id.ToString()));
        }
        catch (Exception)
        {
            return Task.FromResult(ErrorResponseModel<string>.Failure(GenericErrors.TransFailed));
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

            // Add filter parameters
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

            var dt = await _sQLHelper.ExecuteDataTableAsync("[finance].[SP_GetAdditionNotifications]", parameters.ToArray());

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

    public async Task<ErrorResponseModel<AdditionNotificationResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var notification = await _unitOfWork.Repository<AdditionNotice>()
                .GetAll(x => x.Id == id && x.IsActive)
                .Include(x => x.Bank)
                .Include(x => x.Account)
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
                AccountName = notification.Account.Name,
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