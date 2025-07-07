using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.Items;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS;
public class ItemService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IItemService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISQLHelper _sQLHelper = sQLHelper;

    public async Task<ErrorResponseModel<string>> CreateItemAsync(ItemRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var item = new Item
            {
                NameAr = request.NameAr,
                NameEn = request.NameEn,
                UnitId = request.UnitId,
                GroupId = request.GroupId,
                OrderLimit = request.OrderLimit,
                Cost = request.Cost,
                OpeningBalance = request.OpeningBalance,
                SalesTax = request.SalesTax,
                Price = request.Price,
                PriceAfterTax = request.Price + (request.Price * request.SalesTax / 100),
                HasBarcode = request.HasBarcode,
                TypeId = request.TypeId,
                IsActive = true
            };

            await _unitOfWork.Repository<Item>().AddAsync(item, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, item.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<ItemResponse>>> GetItemsAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
    {
        try
        {
            var parameters = new SqlParameter[]
            {
            new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value),
            new SqlParameter("@CurrentPage", pagingFilter.CurrentPage),
            new SqlParameter("@PageSize", pagingFilter.PageSize)
            };

            var dt = await _sQLHelper.ExecuteDataTableAsync("Finance.SP_GetAllItems", parameters);

            var items = dt.AsEnumerable().Select(row => new ItemResponse
            {
                Id = row.Field<int>("Id"),
                NameAr = row.Field<string>("NameAr") ?? string.Empty,
                NameEn = row.Field<string>("NameEn") ?? string.Empty,
                UnitId = row.Field<int>("UnitId"),
                UnitName = row.Field<string>("UnitName") ?? string.Empty,
                GroupId = row.Field<int?>("GroupId"),
                GroupName = row.Field<string>("GroupName"),
                OrderLimit = row.Field<decimal>("OrderLimit"),
                Cost = row.Field<decimal>("Cost"),
                OpeningBalance = row.Field<decimal>("OpeningBalance"),
                SalesTax = row.Field<decimal>("SalesTax"),
                Price = row.Field<decimal>("Price"),
                PriceAfterTax = row.Field<decimal>("PriceAfterTax"),
                HasBarcode = row.Field<bool>("HasBarcode"),
                TypeId = row.Field<int?>("TypeId"),
                TypeName = row.Field<string>("TypeName"),
                IsActive = row.Field<bool>("IsActive"),
                Audit = new AuditResponse
                {
                    CreatedBy = row.Field<string>("CreatedBy") ?? string.Empty,
                    CreatedOn = row.Field<DateTime>("CreatedOn")
                }
            }).ToList();

            int totalCount = dt.Rows.Count > 0 ? dt.Rows[0].Field<int>("TotalCount") : 0;

            return PagedResponseModel<List<ItemResponse>>.Success(
                GenericErrors.GetSuccess,
                totalCount,
                items
            );
        }
        catch (Exception)
        {
            return PagedResponseModel<List<ItemResponse>>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<ItemResponse>> GetItemByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var item = await _unitOfWork.Repository<Item>()
                .GetAll()
                .Include(x => x.Unit)
                .Include(x => x.Group)
                .Include(x => x.Type)
                .Include(x => x.CreatedBy)
                .Include(x=>x.UpdatedBy)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (item == null)
                return ErrorResponseModel<ItemResponse>.Failure(GenericErrors.NotFound);

            var response = new ItemResponse
            {
                Id = item.Id,
                NameAr = item.NameAr,
                NameEn = item.NameEn,
                UnitId = item.UnitId,
                UnitName = item.Unit.NameAr,
                GroupId = item.GroupId,
                GroupName = item.Group?.Name ?? string.Empty,
                OrderLimit = item.OrderLimit,
                Cost = item.Cost,
                OpeningBalance = item.OpeningBalance,
                SalesTax = item.SalesTax,
                Price = item.Price,
                PriceAfterTax = item.PriceAfterTax,
                HasBarcode = item.HasBarcode,
                TypeId = item.TypeId,
                TypeName = item.Type?.NameAr,
                IsActive = item.IsActive,
                Audit = new AuditResponse
                {
                    CreatedOn = item.CreatedOn,
                    CreatedBy = item.CreatedBy.UserName,
                    UpdatedBy = item.UpdatedBy?.UserName,
                    UpdatedOn = item.UpdatedOn
                }
            };

            return ErrorResponseModel<ItemResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<ItemResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateItemAsync(int id, ItemRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var item = await _unitOfWork.Repository<Item>().GetByIdAsync(id, cancellationToken);

            if (item == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            item.NameAr = request.NameAr;
            item.NameEn = request.NameEn;
            item.UnitId = request.UnitId;
            item.GroupId = request.GroupId;
            item.OrderLimit = request.OrderLimit;
            item.Cost = request.Cost;
            item.OpeningBalance = request.OpeningBalance;
            item.SalesTax = request.SalesTax;
            item.Price = request.Price;
            item.PriceAfterTax = request.Price + (request.Price * request.SalesTax / 100);
            item.HasBarcode = request.HasBarcode;
            item.TypeId = request.TypeId;

            _unitOfWork.Repository<Item>().Update(item);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> DeleteItemAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var item = await _unitOfWork.Repository<Item>().GetByIdAsync(id, cancellationToken);

            if (item == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            item.IsActive = false;
            _unitOfWork.Repository<Item>().Update(item);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }
}
