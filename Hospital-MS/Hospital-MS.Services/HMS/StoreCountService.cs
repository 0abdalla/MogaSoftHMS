using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.StoreCount;
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
public class StoreCountService(IUnitOfWork unitOfWork, ISQLHelper sqlHelper) : IStoreCountService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISQLHelper _sqlHelper = sqlHelper;

    public async Task<ErrorResponseModel<string>> CreateAsync(StoreCountRequest request, CancellationToken cancellationToken = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var store = await _unitOfWork.Repository<Store>()
                .GetByIdAsync(request.StoreId, cancellationToken);

            if (store is null)
                return ErrorResponseModel<string>.Failure(new Error("المخزن غير موجود", Status.NotFound));

            var exists = await _unitOfWork.Repository<StoreCount>()
                .AnyAsync(x =>
                    x.StoreId == request.StoreId &&
                    x.FromDate <= request.ToDate &&
                    x.ToDate >= request.FromDate,
                    cancellationToken
                );

            if (exists)
                return ErrorResponseModel<string>.Failure(new Error("يوجد جرد في نفس الفترة", Status.Failed));

            var storeCount = new StoreCount
            {
                StoreId = request.StoreId,
                FromDate = request.FromDate,
                ToDate = request.ToDate
            };

            await _unitOfWork.Repository<StoreCount>().AddAsync(storeCount, cancellationToken);

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
            var parameters = new SqlParameter[]
            {
                new("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value),  
                new("@FromDate", pagingFilter.FilterList?.FirstOrDefault(x => x.CategoryName == "Date")?.FromDate ?? (object)DBNull.Value),
                new("@ToDate", pagingFilter.FilterList?.FirstOrDefault(x => x.CategoryName == "Date")?.ToDate ?? (object)DBNull.Value),
                new("@CurrentPage", pagingFilter.CurrentPage),
                new("@PageSize", pagingFilter.PageSize)
            };

            var dt = await _sqlHelper.ExecuteDataTableAsync("[finance].[SP_GetStoreCounts]", parameters);

            int totalCount = 0;
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["TotalCount"]?.ToString(), out totalCount);
            }

            return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, totalCount, dt);
        }
        catch (Exception)
        {
            return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<StoreCountResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var count = await _unitOfWork.Repository<StoreCount>()
                .GetAll()
                .Include(x => x.Store)
                .Include(x => x.CreatedBy)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (count is null)
                return ErrorResponseModel<StoreCountResponse>.Failure(GenericErrors.NotFound);

            var response = new StoreCountResponse
            {
                Id = count.Id,
                StoreName = count.Store.Name,
                FromDate = count.FromDate,
                ToDate = count.ToDate,
                Audit = new AuditResponse
                {
                    CreatedBy = count.CreatedBy?.UserName,
                    CreatedOn = count.CreatedOn,
                    UpdatedBy = count.UpdatedBy?.UserName,
                    UpdatedOn = count.UpdatedOn
                }
            };

            return ErrorResponseModel<StoreCountResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<StoreCountResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, StoreCountRequest request, CancellationToken cancellationToken = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var count = await _unitOfWork.Repository<StoreCount>()
                .GetByIdAsync(id, cancellationToken);

            if (count is null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            var exists = await _unitOfWork.Repository<StoreCount>()
                .AnyAsync(x =>
                    x.Id != id &&
                    x.StoreId == request.StoreId &&
                    x.FromDate <= request.ToDate &&
                    x.ToDate >= request.FromDate,
                    cancellationToken);

            if (exists)
                return ErrorResponseModel<string>.Failure(new Error("يوجد جرد في نفس الفترة", Status.Failed));

            count.StoreId = request.StoreId;
            count.FromDate = request.FromDate;
            count.ToDate = request.ToDate;

            _unitOfWork.Repository<StoreCount>().Update(count);

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

    public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var count = await _unitOfWork.Repository<StoreCount>()
                .GetByIdAsync(id, cancellationToken);

            if (count is null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            count.IsActive = false;

            _unitOfWork.Repository<StoreCount>().Update(count);

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