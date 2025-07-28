using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.PurchaseOrders;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Hospital_MS.Services.HMS;
public class PurchaseOrderService(IUnitOfWork unitOfWork) : IPurchaseOrderService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorResponseModel<string>> CreateAsync(PurchaseOrderRequest request, CancellationToken cancellationToken = default)
    {
        var order = new PurchaseOrder
        {
            OrderNumber = await GenerateOrderNumber(cancellationToken),
            ReferenceNumber = request.ReferenceNumber,
            OrderDate = request.OrderDate,
            SupplierId = request.SupplierId,
            Description = request.Description,
            Status = PurchaseStatus.Pending,
            PriceQuotationId = request.PriceQuotationId,
            IsActive = true,
            Items = request.Items.Select(i => new PurchaseOrderItem
            {
                ItemId = i.ItemId,
                Unit = i.Unit,
                RequestedQuantity = i.RequestedQuantity,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                IsActive = true,
                TotalPrice = i.TotalPrice,
            }).ToList()
        };

        await _unitOfWork.Repository<PurchaseOrder>().AddAsync(order, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, order.OrderNumber);
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, PurchaseOrderRequest request, CancellationToken cancellationToken = default)
    {
        var order = await _unitOfWork.Repository<PurchaseOrder>()
            .GetAll()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

        if (order == null)
            return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

        order.ReferenceNumber = request.ReferenceNumber;
        order.OrderDate = request.OrderDate;
        order.SupplierId = request.SupplierId;
        order.Description = request.Description;
        order.PriceQuotationId = request.PriceQuotationId;


        foreach (var item in order.Items)
            item.IsActive = false;

        order.Items = request.Items.Select(i => new PurchaseOrderItem
        {
            ItemId = i.ItemId,
            Unit = i.Unit,
            RequestedQuantity = i.RequestedQuantity,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            IsActive = true,
            TotalPrice = i.TotalPrice,
        }).ToList();

        _unitOfWork.Repository<PurchaseOrder>().Update(order);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, order.Id.ToString());
    }

    public async Task<ErrorResponseModel<PurchaseOrderResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _unitOfWork.Repository<PurchaseOrder>()
            .GetAll()
            .Include(x => x.Supplier)
            .Include(x => x.PriceQuotation)
            .Include(x => x.Items)
            .ThenInclude(i => i.Item).ThenInclude(i => i.Unit)
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

        if (order == null)
            return ErrorResponseModel<PurchaseOrderResponse>.Failure(GenericErrors.NotFound);

        var response = new PurchaseOrderResponse
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            ReferenceNumber = order.ReferenceNumber,
            OrderDate = order.OrderDate,
            SupplierId = order.SupplierId,
            SupplierName = order.Supplier.Name,
            PriceQuotationId = order.PriceQuotationId,
            PriceQuotationNumber = order.PriceQuotation?.QuotationNumber,
            Description = order.Description,
            Status = order.Status.ToString(),
            Items = order.Items.Where(i => i.IsActive).Select(i => new PurchaseOrderItemResponse
            {
                Id = i.Id,
                ItemName = i.Item.NameAr,
                Unit = i.Item.Unit.Name,
                RequestedQuantity = i.RequestedQuantity,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Total = i.Quantity * i.UnitPrice,
                TotalPrice = i.TotalPrice
            }).ToList()
        };

        return ErrorResponseModel<PurchaseOrderResponse>.Success(GenericErrors.GetSuccess, response);
    }

    public async Task<PagedResponseModel<List<PurchaseOrderResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        var query = _unitOfWork.Repository<PurchaseOrder>().GetAll()
            .Include(x => x.Supplier)
            .Include(x => x.PriceQuotation)
            .Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(filter.SearchText))
            query = query.Where(x => x.OrderNumber.Contains(filter.SearchText) || x.Description.Contains(filter.SearchText));

        var totalCount = await query.CountAsync(cancellationToken);

        var list = await query
            .OrderByDescending(x => x.Id)
            .Skip((filter.CurrentPage - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x => new PurchaseOrderResponse
            {
                Id = x.Id,
                OrderNumber = x.OrderNumber,
                ReferenceNumber = x.ReferenceNumber,
                OrderDate = x.OrderDate,
                SupplierName = x.Supplier.Name,
                Description = x.Description,
                Status = x.Status.ToString(),
                Items = new(),
                PriceQuotationId = x.PriceQuotationId,
                PriceQuotationNumber = x.PriceQuotation.QuotationNumber
            })
            .ToListAsync(cancellationToken);

        return PagedResponseModel<List<PurchaseOrderResponse>>.Success(GenericErrors.GetSuccess, totalCount, list);
    }

    public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _unitOfWork.Repository<PurchaseOrder>()
            .GetAll()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

        if (order == null)
            return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

        order.IsActive = false;
        foreach (var item in order.Items)
            item.IsActive = false;

        _unitOfWork.Repository<PurchaseOrder>().Update(order);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, order.Id.ToString());
    }

    private async Task<string> GenerateOrderNumber(CancellationToken cancellationToken)
    {
        var year = DateTime.Now.Year;
        var count = await _unitOfWork.Repository<PurchaseOrder>()
            .CountAsync(x => x.OrderDate.Year == year, cancellationToken);
        return $"PO-{year}-{(count + 1):D5}";
    }
}
