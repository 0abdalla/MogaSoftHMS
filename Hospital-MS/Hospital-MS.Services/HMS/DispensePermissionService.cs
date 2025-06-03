using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.DispensePermission;
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
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS;
public class DispensePermissionService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IDispensePermissionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISQLHelper _sQLHelper = sQLHelper;

    public async Task<ErrorResponseModel<string>> CreateAsync(DispensePermissionRequest request, CancellationToken cancellationToken = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var fromStore = await _unitOfWork.Repository<Store>()
                .GetByIdAsync(request.FromStoreId, cancellationToken);

            if (fromStore is null)
                return ErrorResponseModel<string>.Failure(new Error("المخزن المصدر غير موجود", Status.NotFound));

            var toStore = await _unitOfWork.Repository<Store>()
                .GetByIdAsync(request.ToStoreId, cancellationToken);

            if (toStore is null)
                return ErrorResponseModel<string>.Failure(new Error("المخزن الهدف غير موجود", Status.NotFound));

            var item = await _unitOfWork.Repository<Item>().GetByIdAsync(request.ItemId, cancellationToken);

            if (item is null)
                return ErrorResponseModel<string>.Failure(new Error("الصنف غير موجود في المخزن", Status.NotFound));


            var permission = new DispensePermission
            {
                Date = request.Date,
                FromStoreId = request.FromStoreId,
                ToStoreId = request.ToStoreId,
                Quantity = request.Quantity,
                ItemId = request.ItemId,
                Notes = request.Notes,

            };

            await _unitOfWork.Repository<DispensePermission>().AddAsync(permission, cancellationToken);

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

    public async Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
    {
        try
        {
            var Params = new SqlParameter[6];
            var Status = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Status")?.ItemValue;
            var FromDate = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.FromDate;
            var ToDate = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ToDate;

            Params[0] = new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value);
            Params[1] = new SqlParameter("@Status", Status ?? (object)DBNull.Value);
            Params[2] = new SqlParameter("@FromDate", FromDate ?? (object)DBNull.Value);
            Params[3] = new SqlParameter("@ToDate", ToDate ?? (object)DBNull.Value);
            Params[4] = new SqlParameter("@CurrentPage", pagingFilter.CurrentPage);
            Params[5] = new SqlParameter("@PageSize", pagingFilter.PageSize);

            var dt = await _sQLHelper.ExecuteDataTableAsync("[finance].[SP_GetAllIDispensePermission]", Params);

            int totalCount = 0;
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["TotalCount"]?.ToString(), out totalCount);
            }

            foreach (DataRow row in dt.Rows)
            {
                if (row["Status"] != null)
                {
                    row["Status"] = TranslateStatus(row["Status"].ToString());
                }
            }

            return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, totalCount, dt);
        }
        catch (Exception)
        {
            return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<DispensePermissionResponse>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var permission = await _unitOfWork.Repository<DispensePermission>()
                .GetAll()
                .Include(x => x.FromStore)
                .Include(x => x.ToStore)
                .Include(x => x.CreatedBy)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (permission is null)
                return ErrorResponseModel<DispensePermissionResponse>.Failure(GenericErrors.NotFound);

            var response = new DispensePermissionResponse
            {
                Id = permission.Id,
                Date = permission.Date,
                FromStoreName = permission.FromStore.Name,
                ToStoreName = permission.ToStore.Name,
                Quantity = permission.Quantity,
                ItemName = permission.Item.NameAr,
                Balance = permission.Balance,
                Status = permission.Status,
                Notes = permission.Notes,
                Audit = new AuditResponse
                {
                    CreatedBy = permission.CreatedBy?.UserName,
                    CreatedOn = permission.CreatedOn,
                    UpdatedBy = permission.UpdatedBy?.UserName,
                    UpdatedOn = permission.UpdatedOn
                }

            };

            return ErrorResponseModel<DispensePermissionResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<DispensePermissionResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    private string TranslateStatus(string status) => status.ToLower() switch
    {
        "pending" => "قيد الانتظار",
        "in_progress" => "جاري التنفيذ",
        "completed" => "مكتمل",
        "cancelled" => "ملغي",
        _ => status
    };
}