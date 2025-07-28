using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.PurchasePermission;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS;
public class ReceiptPermissionService(IUnitOfWork unitOfWork) : IReceiptPermissionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorResponseModel<string>> CreateAsync(ReceiptPermissionRequest request, CancellationToken cancellationToken = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var store = await _unitOfWork.Repository<Store>()
                .GetByIdAsync(request.StoreId, cancellationToken);
            if (store is null)
                return ErrorResponseModel<string>.Failure(new Error("المخزن غير موجود", Status.NotFound));

            var supplier = await _unitOfWork.Repository<Supplier>()
                .GetByIdAsync(request.SupplierId, cancellationToken);
            if (supplier is null)
                return ErrorResponseModel<string>.Failure(new Error("المورد غير موجود", Status.NotFound));

            var purchaseRequest = await _unitOfWork.Repository<PurchaseOrder>()
                .GetByIdAsync(request.PurchaseOrderId, cancellationToken);
            if (purchaseRequest is null)
                return ErrorResponseModel<string>.Failure(new Error("امر الشراء غير موجود", Status.NotFound));

            var permission = new ReceiptPermission
            {
                PermissionNumber = await GeneratePermissionNumber(cancellationToken),
                DocumentNumber = request.DocumentNumber,
                PermissionDate = DateOnly.FromDateTime(request.PermissionDate),
                StoreId = request.StoreId,
                SupplierId = request.SupplierId,
                PurchaseOrderId = request.PurchaseOrderId,
                Notes = request.Notes,

                Items = request.Items.Select(i => new ReceiptPermissionItem
                {
                    ItemId = i.Id,
                    Unit = i.Unit,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice,
                    IsActive = true
                }).ToList()
            };

            await _unitOfWork.Repository<ReceiptPermission>().AddAsync(permission, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, permission.PermissionNumber);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var permission = await _unitOfWork.Repository<ReceiptPermission>()
                .GetAll()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (permission == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            permission.IsActive = false;
            foreach (var item in permission.Items)
                item.IsActive = false;

            _unitOfWork.Repository<ReceiptPermission>().Update(permission);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, permission.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<ReceiptPermissionResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<ReceiptPermission>()
                .GetAll()
                .Include(x => x.Store)
                .Include(x => x.Supplier)
                .Include(x => x.Items)
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                query = query.Where(x =>
                    x.PermissionNumber.Contains(filter.SearchText) ||
                    x.Notes.Contains(filter.SearchText));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var list = await query
                .OrderByDescending(x => x.Id)
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new ReceiptPermissionResponse
                {
                    Id = x.Id,
                    PermissionNumber = x.PermissionNumber,
                    DocumentNumber = x.DocumentNumber,
                    PermissionDate = x.PermissionDate,
                    Notes = x.Notes,
                    Status = x.Status.ToString(),
                    StoreId = x.StoreId,
                    StoreName = x.Store.Name,
                    SupplierId = x.SupplierId,
                    SupplierName = x.Supplier.Name,
                    Items = x.Items.Where(i => i.IsActive).Select(i => new ReceiptPermissionItemResponse
                    {
                        ItemId = i.ItemId,
                        ItemName = i.Item.NameAr,
                        Unit = i.Unit,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        TotalPrice = i.TotalPrice
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            return PagedResponseModel<List<ReceiptPermissionResponse>>.Success(GenericErrors.GetSuccess, totalCount, list);
        }
        catch (Exception)
        {
            return PagedResponseModel<List<ReceiptPermissionResponse>>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<ReceiptPermissionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var permission = await _unitOfWork.Repository<ReceiptPermission>()
                .GetAll()
                .Include(x => x.Store)
                .Include(x => x.Supplier)
                .Include(x => x.Items)
                .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (permission == null)
                return ErrorResponseModel<ReceiptPermissionResponse>.Failure(GenericErrors.NotFound);

            var response = new ReceiptPermissionResponse
            {
                Id = permission.Id,
                PermissionNumber = permission.PermissionNumber,
                DocumentNumber = permission.DocumentNumber,
                PermissionDate = permission.PermissionDate,
                Notes = permission.Notes,
                SupplierName = permission.Supplier.Name,
                SupplierId = permission.SupplierId,
                StoreName = permission.Store.Name,
                StoreId = permission.StoreId,
                Status = permission.Status.ToString(),
                Items = permission.Items.Where(i => i.IsActive).Select(i => new ReceiptPermissionItemResponse
                {
                    ItemId = i.ItemId,
                    ItemName = i.Item.NameAr,
                    Unit = i.Unit,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList()
            };

            return ErrorResponseModel<ReceiptPermissionResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<ReceiptPermissionResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, ReceiptPermissionRequest request, CancellationToken cancellationToken = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var permission = await _unitOfWork.Repository<ReceiptPermission>()
                .GetAll()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (permission == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            permission.DocumentNumber = request.DocumentNumber;
            permission.PermissionDate = DateOnly.FromDateTime(request.PermissionDate);
            permission.Notes = request.Notes;

            foreach (var item in permission.Items)
                item.IsActive = false;

            permission.Items = request.Items.Select(i => new ReceiptPermissionItem
            {
                ItemId = i.Id,
                Unit = i.Unit,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice,
                IsActive = true
            }).ToList();

            _unitOfWork.Repository<ReceiptPermission>().Update(permission);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, permission.Id.ToString());
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    private async Task<string> GeneratePermissionNumber(CancellationToken cancellationToken)
    {
        var year = DateTime.Now.Year;
        var count = await _unitOfWork.Repository<ReceiptPermission>()
            .CountAsync(x => x.PermissionDate.Year == year, cancellationToken);
        return $"MC-{year}-{(count + 1):D5}";
    }
}
