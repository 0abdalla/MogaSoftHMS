using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.PurchaseOrder;
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
public class PurchaseOrderService(IUnitOfWork unitOfWork, ISQLHelper sqlHelper) : IPurchaseOrderService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISQLHelper _sqlHelper = sqlHelper;

    public async Task<ErrorResponseModel<string>> CreateAsync(PurchaseOrderRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var purchaseOrder = new PurchaseOrder
            {
                OrderNumber = await GenerateOrderNumber(cancellationToken),
                Date = request.Date,
                CostCenterId = request.CostCenterId,
                SupplierId = request.SupplierId,
                StoreId = request.StoreId,
                Currency = request.Currency,
                Notes = request.Notes,
                IsActive = true
            };

            var items = request.Items.Select(item => new PurchaseOrderItem
            {
                ItemId = item.ItemId,
                Quantity = item.Quantity,
                UnitCost = item.UnitCost,
                TotalCost = item.Quantity * item.UnitCost,
                IsActive = true
            }).ToList();

            purchaseOrder.Items = items;
            purchaseOrder.TotalCost = items.Sum(x => x.TotalCost);

            await _unitOfWork.Repository<PurchaseOrder>().AddAsync(purchaseOrder, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, purchaseOrder.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<PurchaseOrderResponse>>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
    {
        try
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value),
                new SqlParameter("@CurrentPage", pagingFilter.CurrentPage),
                new SqlParameter("@PageSize", pagingFilter.PageSize)  
            };

            var dt = await _sqlHelper.ExecuteDataTableAsync("Finance.SP_GetPurchaseOrders", parameters);

            var purchaseOrders = dt.AsEnumerable().Select(row => new PurchaseOrderResponse
            {
                Id = row.Field<int>("Id"),
                OrderNumber = row.Field<string>("OrderNumber"),
                Date = row.Field<DateTime>("Date"),
                CostCenterName = row.Field<string>("CostCenterName"),
                SupplierName = row.Field<string>("SupplierName"),
                StoreName = row.Field<string>("StoreName"),
                Currency = row.Field<string>("Currency"),
                Notes = row.Field<string>("Notes"),
                TotalCost = row.Field<decimal>("TotalCost"),
                IsActive = row.Field<bool>("IsActive"),
                Audit = new AuditResponse
                {
                    CreatedBy = row.Field<string>("CreatedBy"),
                    CreatedOn = row.Field<DateTime>("CreatedOn")
                }
            }).ToList();

            int totalCount = dt.Rows.Count > 0 ? dt.Rows[0].Field<int>("TotalCount") : 0;

            return PagedResponseModel<List<PurchaseOrderResponse>>.Success(GenericErrors.GetSuccess, totalCount, purchaseOrders);
        }
        catch (Exception)
        {
            return PagedResponseModel<List<PurchaseOrderResponse>>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<PurchaseOrderResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var purchaseOrder = await _unitOfWork.Repository<PurchaseOrder>()
                .GetAll()
                .Include(x => x.CostCenter)
                .Include(x => x.Supplier)
                .Include(x => x.Store)
                .Include(x => x.Items)
                    .ThenInclude(x => x.Item)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (purchaseOrder == null)
                return ErrorResponseModel<PurchaseOrderResponse>.Failure(GenericErrors.NotFound);

            var response = new PurchaseOrderResponse
            {
                Id = purchaseOrder.Id,
                OrderNumber = purchaseOrder.OrderNumber,
                Date = purchaseOrder.Date,
                CostCenterName = purchaseOrder.CostCenter.Name,
                SupplierName = purchaseOrder.Supplier.Name,
                StoreName = purchaseOrder.Store.Name,
                Currency = purchaseOrder.Currency,
                Notes = purchaseOrder.Notes,
                TotalCost = purchaseOrder.TotalCost,
                IsActive = purchaseOrder.IsActive,
                Items = [.. purchaseOrder.Items.Select(x => new PurchaseOrderItemResponse
                {
                    Id = x.Id,
                    ItemName = x.Item.NameAr,
                    Quantity = x.Quantity,
                    UnitCost = x.UnitCost,
                    TotalCost = x.TotalCost
                })],
                Audit = new AuditResponse
                {
                    CreatedBy = purchaseOrder.CreatedBy.UserName,
                    CreatedOn = purchaseOrder.CreatedOn,
                    UpdatedBy = purchaseOrder.UpdatedBy?.UserName,
                    UpdatedOn = purchaseOrder.UpdatedOn
                }
            };

            return ErrorResponseModel<PurchaseOrderResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<PurchaseOrderResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, PurchaseOrderRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var purchaseOrder = await _unitOfWork.Repository<PurchaseOrder>()
                .GetAll()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (purchaseOrder == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            purchaseOrder.Date = request.Date;
            purchaseOrder.CostCenterId = request.CostCenterId;
            purchaseOrder.SupplierId = request.SupplierId;
            purchaseOrder.StoreId = request.StoreId;
            purchaseOrder.Currency = request.Currency;
            purchaseOrder.Notes = request.Notes;

            // Remove existing items
            foreach (var item in purchaseOrder.Items.ToList())
            {
                item.IsActive = false;
            }

            // Add new items
            var items = request.Items.Select(item => new PurchaseOrderItem
            {
                ItemId = item.ItemId,
                Quantity = item.Quantity,
                UnitCost = item.UnitCost,
                TotalCost = item.Quantity * item.UnitCost,
                IsActive = true
            }).ToList();

            purchaseOrder.Items = items;
            purchaseOrder.TotalCost = items.Sum(x => x.TotalCost);

            _unitOfWork.Repository<PurchaseOrder>().Update(purchaseOrder);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
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
            var purchaseOrder = await _unitOfWork.Repository<PurchaseOrder>()
                .GetAll()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (purchaseOrder == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            purchaseOrder.IsActive = false;
            foreach (var item in purchaseOrder.Items)
            {
                item.IsActive = false;
            }

            _unitOfWork.Repository<PurchaseOrder>().Update(purchaseOrder);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    private async Task<string> GenerateOrderNumber(CancellationToken cancellationToken)
    {
        var year = DateTime.Now.Year;
        var count = await _unitOfWork.Repository<PurchaseOrder>()
            .CountAsync(x => x.Date.Year == year, cancellationToken);
        return $"PO-{year}-{(count + 1).ToString().PadLeft(5, '0')}";
    }
}
