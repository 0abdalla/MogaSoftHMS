using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.Items;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Wrappers;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Hospital_MS.Services.Specifications;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Hospital_MS.Services.HMS;
public class ItemService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IItemService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISQLHelper _sQLHelper = sQLHelper;

    public async Task<ErrorResponseModel<string>> CreateItemAsync(ItemRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var existingItem = await _unitOfWork.Repository<Item>()
                .GetAll()
                .FirstOrDefaultAsync(x => x.NameAR == request.NameAr && x.NameEN == request.NameEn , cancellationToken);

            if (existingItem != null)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            var item = new Item
            {
                //NameAr = request.NameAr,
                //NameEn = request.NameEn,
                //UnitId = request.UnitId,
                // Unit = request.Unit,
                GroupId = request.GroupId,
                //OrderLimit = request.OrderLimit,
                Cost = request.Cost,
                //OpeningBalance = request.OpeningBalance,
                SalesTax = request.SalesTax,
                Price = request.Price,
                //PriceAfterTax = request.Price + (request.Price * request.SalesTax / 100),
                HasBarcode = request.HasBarcode,
                //TypeId = request.TypeId,

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

    public async Task<PagedResponseModel<List<ItemResponse>>> GetAllItemsAsync(SearchRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<Item>()
                .GetAllAsQueryable()
                .Where(i => i.IsDeleted == false)
                .Include(i => i.Unit)
                .Include(i => i.Group)
                .ThenInclude(g => g.MainGroup)
                .Include(i => i.CreatedBy)
                .Include(i => i.UpdatedBy)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(i =>
                    i.NameAR.Contains(request.SearchTerm) ||
                    i.NameEN.Contains(request.SearchTerm));
            }

            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                if (request.SortDescending)
                    query = query.OrderByDescending(e => EF.Property<object>(e, request.SortBy));
                else
                    query = query.OrderBy(e => EF.Property<object>(e, request.SortBy));
            }
            else
            {
                query = query.OrderByDescending(i => i.Id);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var skip = (request.PageNumber - 1) * request.PageSize;

            var data = await query
                .Skip(skip)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var items = data.Select(i => new ItemResponse
            {
                Id = i.Id,
                NameAr = i.NameAR,
                NameEn = i.NameEN,

                UnitId = i.UnitId,
                UnitName = i.Unit?.Name ?? string.Empty,

                GroupId = i.GroupId,
                GroupName = i.Group?.Name ?? string.Empty,


                OrderLimit = i.OrderLimit,
                Cost = i.Cost,
                OpeningBalance = i.OpeningBalance,
                SalesTax = i.SalesTax,
                Price = i.Price,
                PriceAfterTax = i.PriceAfterTax,
                HasBarcode = i.HasBarcode,


                Audit = new AuditResponse
                {
                    CreatedBy = i.CreatedBy?.UserName,
                    UpdatedBy = i.UpdatedBy?.UserName,
                    CreatedOn = i.CreatedOn,
                    IsDeleted = i.IsDeleted
                }
            }).ToList();

            return PagedResponseModel<List<ItemResponse>>
                .Success(GenericErrors.GetSuccess, totalCount, items);
        }
        catch (Exception)
        {
            return PagedResponseModel<List<ItemResponse>>
                .Failure(GenericErrors.TransFailed);
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
                .Include(x => x.Unit)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(x => x.Id == id , cancellationToken);

            if (item == null)
                return ErrorResponseModel<ItemResponse>.Failure(GenericErrors.NotFound);

            var response = new ItemResponse
            {
                Id = item.Id,
                NameAr = item.NameAR,
                NameEn = item.NameEN,
                UnitId = item.UnitId,
                UnitName = item?.Unit?.Name,
                //Unit = item.Unit,
                GroupId = item.GroupId,
                GroupName = item.Group?.Name ?? string.Empty,
                OrderLimit = item.OrderLimit,
                Cost = item.Cost,
                OpeningBalance = item.OpeningBalance,
                SalesTax = item.SalesTax,
                Price = item.Price,
                PriceAfterTax = item.PriceAfterTax,
                HasBarcode = item.HasBarcode,

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

            item.NameAR = request.NameAr;
            item.NameEN = request.NameEn;
            //item.Unit = request.Unit;
            item.UnitId = request.UnitId;
            item.GroupId = request.GroupId;
            item.Cost = request.Cost;
            item.SalesTax = request.SalesTax;
            item.Price = request.Price;

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

            item.IsDeleted = !item.IsDeleted;
            _unitOfWork.Repository<Item>().Update(item);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<ItemMovementResult>> GetItemMovementAsync(int id, GetItemMovementRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var item = await _unitOfWork.Repository<Item>()
                .GetAll(x => x.Id == id )
                .Include(x => x.Unit)
                .Include(x => x.Group)
                .ThenInclude(g => g.MainGroup)
                .FirstOrDefaultAsync(cancellationToken);

            if (item == null)
                return ErrorResponseModel<ItemMovementResult>.Failure(GenericErrors.NotFound);

            var store = await
                _unitOfWork.Repository<Store>()
                .GetAll(x => x.Id == request.StoreId )
                .FirstOrDefaultAsync(cancellationToken);

            if (store == null)
                return ErrorResponseModel<ItemMovementResult>.Failure(GenericErrors.NotFound);


            var fromDate = request.FromDate;
            var toDate = request.ToDate;

            var itemReceiptPermissions = await _unitOfWork.Repository<ReceiptPermissionItem>()
                .GetAll(x => x.ReceiptPermission.StoreId == request.StoreId
                            && x.ItemId == id
                            && x.ReceiptPermission.PermissionDate >= fromDate
                            && x.ReceiptPermission.PermissionDate <= toDate)
                .Include(x => x.Item)
                .Include(x => x.ReceiptPermission)
                .ThenInclude(x => x.Supplier)
                .ToListAsync(cancellationToken);

            var itemMaterialIssues = await _unitOfWork.Repository<MaterialIssueItem>()
                .GetAll(x => x.MaterialIssuePermission.StoreId == request.StoreId
                            && x.ItemId == id
                            && x.MaterialIssuePermission.PermissionDate >= fromDate
                            && x.MaterialIssuePermission.PermissionDate <= toDate)
                .Include(x => x.Item)
                .Include(x => x.MaterialIssuePermission)
                .ToListAsync(cancellationToken);

            var previousReceipts = await _unitOfWork.Repository<ReceiptPermissionItem>()
                .GetAll(x => x.ReceiptPermission.StoreId == request.StoreId
                             && x.ItemId == id
                             && x.ReceiptPermission.PermissionDate < fromDate)
                .ToListAsync(cancellationToken);

            var previousIssues = await _unitOfWork.Repository<MaterialIssueItem>()
                .GetAll(x => x.MaterialIssuePermission.StoreId == request.StoreId
                             && x.ItemId == id
                             && x.MaterialIssuePermission.PermissionDate < fromDate)
                .ToListAsync(cancellationToken);

            var openingBalance = item.OpeningBalance;

            var previousReceiptsValue = previousReceipts.Sum(x => x.Quantity * x.UnitPrice);
            var previousIssuesValue = previousIssues.Sum(x => x.Quantity * x.UnitPrice);
            var previousBalance = openingBalance + previousReceiptsValue - previousIssuesValue;

            //var itemMovements = new List<ItemMovementResult>();


            var receiptsResponse = itemReceiptPermissions.Select(x => new ReceiptItemsResponse
            {
                Date = x.ReceiptPermission.PermissionDate,
                DocumentNumber = x.ReceiptPermission.DocumentNumber,
                ReceiptNumber = x.ReceiptPermission.PermissionNumber,
                SupplierName = x.ReceiptPermission.Supplier.Name,
                TotalReceiptsQuantity = itemReceiptPermissions.Sum(z => z.Quantity),
                TotalPrice = itemReceiptPermissions.Sum(z => z.Quantity * z.UnitPrice),
                ReceiptType = "تحويلات مخازن",
                UnitPrice = x.UnitPrice,
            }).ToList();

            var issuesResponse = itemMaterialIssues.Select(x => new IssueItemsResponse
            {
                Date = x.MaterialIssuePermission.PermissionDate,
                DocumentNumber = x.MaterialIssuePermission.DocumentNumber,
                InvoiceNumber = "",
                IssueId = x.MaterialIssuePermission.Id,
                IssueType = "صرف مبيعات",
                SupplierName = "",
                TotalIssuesQuantity = itemMaterialIssues.Sum(x => x.Quantity),
                TotalPrice = itemMaterialIssues.Sum(x => x.Quantity * x.UnitPrice),
                UnitPrice = x.UnitPrice
            }).ToList();

            var totalItemIssues = itemMaterialIssues.Sum(x => x.Quantity);
            var totalItemReceipts = itemReceiptPermissions.Sum(x => x.Quantity);

            var itemMovement = new ItemMovementResult
            {
                ItemId = id,
                ItemName = item.NameAR ?? item.NameEN,
                ItemGroupName = item.Group?.Name ?? string.Empty,
                ItemUnit = item.Unit?.Name ?? string.Empty,
                ItemIssues = issuesResponse,
                ItemReceipts = receiptsResponse,
                PreviousBalance = previousBalance,
                TotalItemIssues = totalItemIssues,
                TotalItemReceipts = totalItemReceipts,
                StoreName = store.Name
            };

            return ErrorResponseModel<ItemMovementResult>.Success(GenericErrors.GetSuccess, itemMovement);
        }
        catch (Exception)
        {
            return ErrorResponseModel<ItemMovementResult>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<ItemMovementResult>> GetItemMovementAsyncV2(int id, GetItemMovementRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var item = await _unitOfWork.Repository<Item>()
                .GetAll(x => x.Id == id)
                .Include(x => x.Unit)
                .Include(x => x.Group).ThenInclude(g => g.MainGroup)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (item == null)
                return ErrorResponseModel<ItemMovementResult>.Failure(GenericErrors.NotFound);

            var store = await _unitOfWork.Repository<Store>()
                .GetAll(x => x.Id == request.StoreId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (store == null)
                return ErrorResponseModel<ItemMovementResult>.Failure(GenericErrors.NotFound);

            var fromDate = request.FromDate;
            var toDate = request.ToDate;

            var receiptItems = await _unitOfWork.Repository<ReceiptPermissionItem>()
                .GetAll(x => x.ItemId == id &&
                             x.ReceiptPermission.StoreId == request.StoreId &&
                             x.ReceiptPermission.PermissionDate >= fromDate &&
                             x.ReceiptPermission.PermissionDate <= toDate)
                .Include(x => x.ReceiptPermission).ThenInclude(p => p.Supplier)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var issueItems = await _unitOfWork.Repository<MaterialIssueItem>()
                .GetAll(x => x.ItemId == id &&
                             x.MaterialIssuePermission.StoreId == request.StoreId &&
                             x.MaterialIssuePermission.PermissionDate >= fromDate &&
                             x.MaterialIssuePermission.PermissionDate <= toDate)
                .Include(x => x.MaterialIssuePermission)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var previousReceiptsValue = await _unitOfWork.Repository<ReceiptPermissionItem>()
                .GetAll(x => x.ItemId == id &&
                             x.ReceiptPermission.StoreId == request.StoreId &&
                             x.ReceiptPermission.PermissionDate < fromDate)
                .AsNoTracking()
                .SumAsync(x => x.Quantity * x.UnitPrice, cancellationToken);

            var previousIssuesValue = await _unitOfWork.Repository<MaterialIssueItem>()
                .GetAll(x => x.ItemId == id &&
                             x.MaterialIssuePermission.StoreId == request.StoreId &&
                             x.MaterialIssuePermission.PermissionDate < fromDate)
                .AsNoTracking()
                .SumAsync(x => x.Quantity * x.UnitPrice, cancellationToken);

            var openingBalance = item.OpeningBalance;
            var previousBalance = openingBalance + previousReceiptsValue - previousIssuesValue;

            var totalReceiptsQuantity = receiptItems.Sum(x => x.Quantity);
            var totalReceiptsValue = receiptItems.Sum(x => x.Quantity * x.UnitPrice);

            var receiptsResponse = receiptItems.Select(x => new ReceiptItemsResponse
            {
                Date = x.ReceiptPermission.PermissionDate,
                DocumentNumber = x.ReceiptPermission.DocumentNumber,
                ReceiptNumber = x.ReceiptPermission.PermissionNumber,
                SupplierName = x.ReceiptPermission.Supplier?.Name ?? "",
                TotalReceiptsQuantity = totalReceiptsQuantity,
                TotalPrice = totalReceiptsValue,
                ReceiptType = "تحويلات مخازن",
                UnitPrice = x.UnitPrice,
            }).ToList();

            var totalIssuesQuantity = issueItems.Sum(x => x.Quantity);
            var totalIssuesValue = issueItems.Sum(x => x.Quantity * x.UnitPrice);

            var issuesResponse = issueItems.Select(x => new IssueItemsResponse
            {
                Date = x.MaterialIssuePermission.PermissionDate,
                DocumentNumber = x.MaterialIssuePermission.DocumentNumber,
                InvoiceNumber = "",
                IssueId = x.MaterialIssuePermission.Id,
                IssueType = "صرف مبيعات",
                SupplierName = "",
                TotalIssuesQuantity = totalIssuesQuantity,
                TotalPrice = totalIssuesValue,
                UnitPrice = x.UnitPrice
            }).ToList();

            var result = new ItemMovementResult
            {
                ItemId = id,
                ItemName = item.NameAR ?? item.NameEN,
                ItemGroupName = item.Group?.Name ?? "",
                ItemUnit = item.Unit?.Name ?? "",
                ItemIssues = issuesResponse,
                ItemReceipts = receiptsResponse,
                PreviousBalance = previousBalance,
                TotalItemIssues = totalIssuesQuantity,
                TotalItemReceipts = totalReceiptsQuantity,
                StoreName = store.Name
            };

            return ErrorResponseModel<ItemMovementResult>.Success(GenericErrors.GetSuccess, result);
        }
        catch
        {
            return ErrorResponseModel<ItemMovementResult>.Failure(GenericErrors.TransFailed);
        }
    }
}
