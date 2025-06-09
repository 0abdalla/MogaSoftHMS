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

    public async Task<PagedResponseModel<List<AdditionNotificationResponse>>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
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
                    parameters.Add(new SqlParameter("@BankId", bankFilter.ItemValue));
                }

                var accountFilter = pagingFilter.FilterList.FirstOrDefault(f => f.CategoryName == "Account");
                if (accountFilter != null)
                {
                    parameters.Add(new SqlParameter("@AccountId", accountFilter.ItemValue));
                }

                var dateFilter = pagingFilter.FilterList.FirstOrDefault(f => f.CategoryName == "Date");
                if (dateFilter != null)
                {
                    if (dateFilter.FromDate.HasValue)
                        parameters.Add(new SqlParameter("@FromDate", dateFilter.FromDate.Value));
                    if (dateFilter.ToDate.HasValue)
                        parameters.Add(new SqlParameter("@ToDate", dateFilter.ToDate.Value));
                }

                var checkNumberFilter = pagingFilter.FilterList.FirstOrDefault(f => f.CategoryName == "CheckNumber");
                if (checkNumberFilter != null)
                {
                    parameters.Add(new SqlParameter("@CheckNumber", checkNumberFilter.ItemValue));
                }
            }

            var dt = await _sQLHelper.ExecuteDataTableAsync("[finance].[SP_GetAdditionNotifications]", parameters.ToArray());

            var notifications = new List<AdditionNotificationResponse>();
            int totalCount = 0;

            if (dt.Rows.Count > 0)
            {
                totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"]);

                notifications = dt.AsEnumerable().Select(row => new AdditionNotificationResponse
                {
                    Id = row.Field<int>("Id"),
                    Date = DateOnly.FromDateTime(row.Field<DateTime>("Date")),
                    BankId = row.Field<int>("BankId"),
                    BankName = row.Field<string>("BankName") ?? string.Empty,
                    AccountId = row.Field<int>("AccountId"),
                    AccountName = row.Field<string>("AccountName") ?? string.Empty,
                    CheckNumber = row.Field<string>("CheckNumber") ?? string.Empty,
                    Amount = row.Field<decimal>("Amount"),
                    Notes = row.Field<string>("Notes")

                }).ToList();
            }

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