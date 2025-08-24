using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.ItemGroups;
using Hospital_MS.Core.Contracts.Items;
using Hospital_MS.Core.Contracts.Stores;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS;
public class StoreService(IUnitOfWork unitOfWork) : IStoreService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorResponseModel<string>> CreateAsync(CreateStoreRequest request, CancellationToken cancellationToken = default)
    {
        var exists = await _unitOfWork.Repository<Store>()
            .AnyAsync(x => x.Name == request.Name || x.Code == request.Code, cancellationToken);

        if (exists)
            return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

        var store = new Store
        {
            Name = request.Name,
            Code = request.Code,
            TypeId = request.StoreTypeId,
            IsActive = true
        };

        await _unitOfWork.Repository<Store>().AddAsync(store, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return ErrorResponseModel<string>.Success(GenericErrors.GetSuccess, store.Id.ToString());
    }
    public async Task<ErrorResponseModel<StoreResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var store = await _unitOfWork.Repository<Store>()
            .GetAll()
            .Include(x => x.Type)
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

        if (store == null)
            return ErrorResponseModel<StoreResponse>.Failure(GenericErrors.NotFound);

        var response = new StoreResponse
        {
            Id = store.Id,
            Name = store.Name,
            Code = store.Code,
            Location = store.Location,
            ContactNumber = store.ContactNumber,
            Email = store.Email,
            StoreTypeId = store.TypeId,
            StoreTypeName = store.Type?.Name,
            IsActive = store.IsActive
        };

        return ErrorResponseModel<StoreResponse>.Success(GenericErrors.GetSuccess, response);
    }

    public async Task<PagedResponseModel<List<StoreResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        var query = _unitOfWork.Repository<Store>()
            .GetAll()
            .Include(x => x.Type)
            .OrderByDescending(x => x.Id)
            .Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(filter.SearchText))
        {
            query = query.Where(x => x.Name.Contains(filter.SearchText) || x.Code.Contains(filter.SearchText));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var stores = await query
            .OrderByDescending(x => x.Id)
            .Skip((filter.CurrentPage - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x => new StoreResponse
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Location = x.Location,
                ContactNumber = x.ContactNumber,
                Email = x.Email,
                StoreTypeId = x.TypeId,
                StoreTypeName = x.Type.Name,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);

        return PagedResponseModel<List<StoreResponse>>.Success(GenericErrors.GetSuccess, totalCount, stores);
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, CreateStoreRequest request, CancellationToken cancellationToken = default)
    {
        var store = await _unitOfWork.Repository<Store>().GetByIdAsync(id, cancellationToken);

        if (store == null)
            return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

        var exists = await _unitOfWork.Repository<Store>()
            .AnyAsync(x => x.Id != id && (x.Name == request.Name || x.Code == request.Code), cancellationToken);

        if (exists)
            return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

        store.Name = request.Name;
        store.Code = request.Code;
        store.TypeId = request.StoreTypeId;

        _unitOfWork.Repository<Store>().Update(store);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, store.Id.ToString());
    }

    public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var store = await _unitOfWork.Repository<Store>().GetByIdAsync(id, cancellationToken);

        if (store == null)
            return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

        store.IsActive = false;
        _unitOfWork.Repository<Store>().Update(store);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, store.Id.ToString());
    }

    public async Task<ErrorResponseModel<List<StoreMovementResponse>>> GetStoreMovementsAsync(int storeId, GetStoresMovementsRequest request, CancellationToken cancellationToken = default)
    {
        var store = await _unitOfWork.Repository<Store>()
                .GetAll()
                .Include(x => x.Type)
                .FirstOrDefaultAsync(x => x.Id == storeId && x.IsActive, cancellationToken);

        if (store == null)
            return ErrorResponseModel<List<StoreMovementResponse>>.Failure(GenericErrors.NotFound);

        var lastReceiptPermissionNo = await _unitOfWork.Repository<ReceiptPermission>()
                .GetAll(x => x.StoreId == storeId)
                .OrderByDescending(x => x.Id)
                .Select(x => x.PermissionNumber)
                .FirstOrDefaultAsync(cancellationToken);

        var lastIssuePermissionNo = await _unitOfWork.Repository<MaterialIssuePermission>()
                .GetAll(x => x.StoreId == storeId)
                .OrderByDescending(x => x.Id)
                .Select(x => x.PermissionNumber)
                .FirstOrDefaultAsync(cancellationToken);

        var fromDate = request.fromDate;
        var toDate = request.toDate;

        var receiptItemIds = await _unitOfWork.Repository<ReceiptPermissionItem>()
                .GetAll(x => x.ReceiptPermission.StoreId == storeId
                          && x.ReceiptPermission.PermissionDate >= fromDate
                          && x.ReceiptPermission.PermissionDate <= toDate)
                .Include(x => x.ReceiptPermission)
                .Select(x => x.ItemId)
                .Distinct()
                .ToListAsync(cancellationToken);

        var issueItemIds = await _unitOfWork.Repository<MaterialIssueItem>()
                .GetAll(x => x.MaterialIssuePermission.StoreId == storeId
                          && x.MaterialIssuePermission.PermissionDate >= fromDate
                          && x.MaterialIssuePermission.PermissionDate <= toDate)
                .Include(x => x.MaterialIssuePermission)
                .Select(x => x.ItemId)
                .Distinct()
                .ToListAsync(cancellationToken);

        var allItemIds = receiptItemIds.Union(issueItemIds).Distinct().ToList();

        var itemsQuery = _unitOfWork.Repository<Item>()
                .GetAll(x => allItemIds.Contains(x.Id) && x.IsActive)
                .Include(x => x.Group)
                .ThenInclude(g => g.MainGroup)
                .AsQueryable();

        var items = await itemsQuery.ToListAsync(cancellationToken);


        var receiptDetails = await _unitOfWork.Repository<ReceiptPermissionItem>()
                .GetAll(x => x.ReceiptPermission.StoreId == storeId
                          && x.ReceiptPermission.PermissionDate >= fromDate
                          && x.ReceiptPermission.PermissionDate <= toDate)
                .Include(x => x.ReceiptPermission)
                .ToListAsync(cancellationToken);

        var issueDetails = await _unitOfWork.Repository<MaterialIssueItem>()
                .GetAll(x => x.MaterialIssuePermission.StoreId == storeId
                          && x.MaterialIssuePermission.PermissionDate >= fromDate
                          && x.MaterialIssuePermission.PermissionDate <= toDate)
                .Include(x => x.MaterialIssuePermission)
                .ToListAsync(cancellationToken);


        var itemMovements = items.Select(item =>
        {
            var openingBalance = item.OpeningBalance;

            var receivedBalance = receiptDetails.Where(r => r.ItemId == item.Id).Sum(r => r.Quantity);
            var issueBalance = issueDetails.Where(i => i.ItemId == item.Id).Sum(i => i.Quantity);
            var totalBalance = openingBalance + receivedBalance - issueBalance;

            return new ItemMovementResponse
            {
                ItemId = item.Id,
                ItemName = item.NameAr,
                OpeningBalance = openingBalance,
                ReceivedBalance = receivedBalance,
                IssueBalance = issueBalance,
                TotalBalance = totalBalance
            };

        }).ToList();


        var filteredItems = items.AsQueryable();

        if (request.MainGroupId.HasValue)
            filteredItems = filteredItems.Where(x => x.Group.MainGroupId == request.MainGroupId.Value);

        if (request.ItemGroupId.HasValue)
            filteredItems = filteredItems.Where(x => x.GroupId == request.ItemGroupId.Value);

        var groupedResult = filteredItems
            .GroupBy(x => x.Group.MainGroup)
            .Select(mainGroup => new StoreMovementResponse
            {
                MainGroupId = mainGroup.Key.Id,
                MainGroupName = mainGroup.Key.Name,
                ItemGroups = mainGroup
                    .GroupBy(x => x.Group)
                    .Select(itemGroup => new ItemGroupsResponse
                    {
                        ItemGroupId = itemGroup.Key.Id,
                        ItemGroupName = itemGroup.Key.Name,
                        Items = itemMovements
                            .Where(m => itemGroup.Any(i => i.Id == m.ItemId))
                            .ToList()
                    }).ToList(),
                LastReceiptPermissionNumber = lastReceiptPermissionNo,
                MaterialIssuePermissionNumber = lastIssuePermissionNo
            }).ToList();

        return ErrorResponseModel<List<StoreMovementResponse>>.Success(GenericErrors.GetSuccess, groupedResult);
    }

    public async Task<ErrorResponseModel<List<ItemsOrderLimitResponse>>> GetItemsOrderLimitAsync(int storeId, CancellationToken cancellationToken = default)
    {
        //var store = await _unitOfWork.Repository<Store>()
        //    .GetAll()
        //    .FirstOrDefaultAsync(x => x.Id == storeId, cancellationToken);

        //if (store == null)
        //    return ErrorResponseModel<List<ItemsOrderLimitResponse>>.Failure(GenericErrors.NotFound);


        var items = await _unitOfWork.Repository<Item>()
            .GetAll(x => x.IsActive)
            .Include(x => x.Group)
            .ToListAsync(cancellationToken);

        var itemIds = items.Select(x => x.Id).ToList();

        var receipts = await _unitOfWork.Repository<ReceiptPermissionItem>()
            .GetAll(x => x.ReceiptPermission.StoreId == storeId && x.IsActive && itemIds.Contains(x.ItemId))
            .Include(x => x.ReceiptPermission)
            .ToListAsync(cancellationToken);

        var issues = await _unitOfWork.Repository<MaterialIssueItem>()
            .GetAll(x => x.MaterialIssuePermission.StoreId == storeId && x.IsActive && itemIds.Contains(x.ItemId))
            .Include(x => x.MaterialIssuePermission)
            .ToListAsync(cancellationToken);

        // Group by ItemGroup
        var result = items
            .GroupBy(x => x.Group)
            .Select(g =>
            {
                var groupItems = g.ToList();
                var groupItemIds = groupItems.Select(i => i.Id).ToList();

                var itemLimits = groupItems.Select(item =>
                {
                    var received = receipts.Where(r => r.ItemId == item.Id).Sum(r => r.Quantity);
                    var issued = issues.Where(i => i.ItemId == item.Id).Sum(i => i.Quantity);
                    var balance = item.OpeningBalance + received - issued;

                    return new ItemLimitsResponse
                    {
                        ItemId = item.Id,
                        ItemName = item.NameAr,
                        OrderLimit = item.OrderLimit,
                        Balance = balance
                    };

                }).ToList();

                return new ItemsOrderLimitResponse
                {
                    ItemGroupId = g.Key.Id,
                    ItemGroupName = g.Key.Name,
                    Items = itemLimits,
                    //StoreId = store?.Id,
                    //StoreName = store?.Name
                };
            })
            .ToList();

        return ErrorResponseModel<List<ItemsOrderLimitResponse>>.Success(GenericErrors.GetSuccess, result);
    }

    public async Task<ErrorResponseModel<List<StoreRateResponse>>> GetStoreRateAsync(int storeId, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.Repository<Item>()
            .GetAll(x => x.IsActive && x.GroupId != null)
            .Include(x => x.Group)
            .Where(x => x.Group != null)
            .ToListAsync(cancellationToken);

        var itemIds = items.Select(x => x.Id).ToList();

        var receipts = await _unitOfWork.Repository<ReceiptPermissionItem>()
            .GetAll(x =>
                x.ReceiptPermission.StoreId == storeId &&
                x.IsActive &&
                itemIds.Contains(x.ItemId) &&
                x.ReceiptPermission.PermissionDate >= fromDate &&
                x.ReceiptPermission.PermissionDate <= toDate)
            .Include(x => x.ReceiptPermission)
            .ToListAsync(cancellationToken);

        var issues = await _unitOfWork.Repository<MaterialIssueItem>()
            .GetAll(x =>
                x.MaterialIssuePermission.StoreId == storeId &&
                x.IsActive &&
                itemIds.Contains(x.ItemId) &&
                x.MaterialIssuePermission.PermissionDate >= fromDate &&
                x.MaterialIssuePermission.PermissionDate <= toDate)
            .Include(x => x.MaterialIssuePermission)
            .ToListAsync(cancellationToken);

        // Group by ItemGroup
        var result = items
            .GroupBy(x => x.Group)
            .Select(g =>
            {
                var groupItems = g.ToList();

                var rateItems = groupItems.Select(item =>
                {
                    var received = receipts.Where(r => r.ItemId == item.Id).Sum(r => r.Quantity);
                    var issued = issues.Where(i => i.ItemId == item.Id).Sum(i => i.Quantity);
                    var balance = item.OpeningBalance + received - issued;
                    var totalAmount = balance * item.Cost;

                    return new StoreRateItemsResponse
                    {
                        ItemId = item.Id,
                        ItemName = item.NameAr,
                        Balance = balance,
                        TotalAmount = totalAmount
                    };
                }).ToList();

                return new StoreRateResponse
                {
                    ItemGroupId = g.Key.Id,
                    ItemGroupName = g.Key.Name,
                    Items = rateItems
                };
            })
            .ToList();

        return ErrorResponseModel<List<StoreRateResponse>>.Success(GenericErrors.GetSuccess, result);
    }

    public async Task<ErrorResponseModel<List<StoreRateResponseV2>>> GetStoreRateAsyncV2(int storeId, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default)
    {
        var store = await _unitOfWork.Repository<Store>()
            .GetAll(x => x.Id == storeId)
            .FirstOrDefaultAsync(cancellationToken);

        if (store == null)
            return ErrorResponseModel<List<StoreRateResponseV2>>.Failure(GenericErrors.NotFound);


        var storeItemIds = await _unitOfWork.Repository<ReceiptPermissionItem>()
            .GetAll(x =>
                x.ReceiptPermission.StoreId == storeId &&
                x.IsActive &&
                x.ReceiptPermission.PermissionDate >= fromDate &&
                x.ReceiptPermission.PermissionDate <= toDate)
            .Select(x => x.ItemId)
            .Union(
                _unitOfWork.Repository<MaterialIssueItem>()
                .GetAll(x =>
                    x.MaterialIssuePermission.StoreId == storeId &&
                    x.IsActive &&
                    x.MaterialIssuePermission.PermissionDate >= fromDate &&
                    x.MaterialIssuePermission.PermissionDate <= toDate)
                .Select(x => x.ItemId)
            )
            .Distinct()
            .ToListAsync(cancellationToken);

        if (!storeItemIds.Any())
            return ErrorResponseModel<List<StoreRateResponseV2>>.Success(GenericErrors.GetSuccess, new List<StoreRateResponseV2>());

        // Get only the items linked to this store
        var items = await _unitOfWork.Repository<Item>()
            .GetAll(x => x.IsActive && storeItemIds.Contains(x.Id) && x.GroupId != null)
            .Include(x => x.Group)
                .ThenInclude(g => g.MainGroup)
            .ToListAsync(cancellationToken);


        var receipts = await _unitOfWork.Repository<ReceiptPermissionItem>()
            .GetAll(x =>
                x.ReceiptPermission.StoreId == storeId &&
                x.IsActive &&
                storeItemIds.Contains(x.ItemId) &&
                x.ReceiptPermission.PermissionDate >= fromDate &&
                x.ReceiptPermission.PermissionDate <= toDate)
            .Include(x => x.ReceiptPermission)
            .ToListAsync(cancellationToken);


        var issues = await _unitOfWork.Repository<MaterialIssueItem>()
            .GetAll(x =>
                x.MaterialIssuePermission.StoreId == storeId &&
                x.IsActive &&
                storeItemIds.Contains(x.ItemId) &&
                x.MaterialIssuePermission.PermissionDate >= fromDate &&
                x.MaterialIssuePermission.PermissionDate <= toDate)
            .Include(x => x.MaterialIssuePermission)
            .ToListAsync(cancellationToken);

        // Group by MainGroup → ItemGroup
        var result = items
            .Where(x => x.Group != null && x.Group.MainGroup != null)
            .GroupBy(x => x.Group.MainGroup)
            .Select(mainGroup =>
            {
                var itemGroups = mainGroup
                    .GroupBy(i => i.Group)
                    .Select(g =>
                    {
                        var groupItems = g.ToList();

                        var rateItems = groupItems.Select(item =>
                        {
                            var received = receipts.Where(r => r.ItemId == item.Id).Sum(r => r.Quantity);
                            var issued = issues.Where(i => i.ItemId == item.Id).Sum(i => i.Quantity);
                            var balance = item.OpeningBalance + received - issued;
                            var totalAmount = balance * item.Cost;

                            return new StoreRateItemsResponseV2
                            {
                                ItemId = item.Id,
                                ItemName = item.NameAr,
                                Balance = balance,
                                TotalAmount = totalAmount
                            };
                        }).ToList();

                        return new StoreRateItemGroupResponseV2
                        {
                            ItemGroupId = g.Key.Id,
                            ItemGroupName = g.Key.Name,
                            Items = rateItems
                        };
                    })
                    .ToList();

                return new StoreRateResponseV2
                {
                    MainGroupId = mainGroup.Key.Id,
                    MainGroupName = mainGroup.Key.Name,
                    StoreId = store.Id,
                    StoreName = store.Name,
                    ItemGroups = itemGroups
                };
            })
            .ToList();

        return ErrorResponseModel<List<StoreRateResponseV2>>.Success(GenericErrors.GetSuccess, result);
    }

    public async Task<ErrorResponseModel<List<MainGroupResponseV2>>> GetItemsOrderLimitAsyncV2(int storeId, CancellationToken cancellationToken = default)
    {
        var store = await _unitOfWork.Repository<Store>()
            .GetAll(x => x.Id == storeId)
            .FirstOrDefaultAsync(cancellationToken);

        if (store == null)
            return ErrorResponseModel<List<MainGroupResponseV2>>.Failure(GenericErrors.NotFound);

        var storeItemIds = await _unitOfWork.Repository<ReceiptPermissionItem>()
            .GetAll(x => x.ReceiptPermission.StoreId == storeId && x.IsActive)
            .Select(x => x.ItemId)
            .Union(
                _unitOfWork.Repository<MaterialIssueItem>()
                .GetAll(x => x.MaterialIssuePermission.StoreId == storeId && x.IsActive)
                .Select(x => x.ItemId)
            )
            .Distinct()
            .ToListAsync(cancellationToken);

        if (!storeItemIds.Any())
            return ErrorResponseModel<List<MainGroupResponseV2>>.Success(GenericErrors.GetSuccess, new List<MainGroupResponseV2>());

        var items = await _unitOfWork.Repository<Item>()
            .GetAll(x => x.IsActive && storeItemIds.Contains(x.Id) && x.GroupId != null)
            .Include(x => x.Group)
                .ThenInclude(g => g.MainGroup)
            .ToListAsync(cancellationToken);

        var receipts = await _unitOfWork.Repository<ReceiptPermissionItem>()
            .GetAll(x => x.ReceiptPermission.StoreId == storeId && x.IsActive && storeItemIds.Contains(x.ItemId))
            .Include(x => x.ReceiptPermission)
            .ToListAsync(cancellationToken);

        var issues = await _unitOfWork.Repository<MaterialIssueItem>()
            .GetAll(x => x.MaterialIssuePermission.StoreId == storeId && x.IsActive && storeItemIds.Contains(x.ItemId))
            .Include(x => x.MaterialIssuePermission)
            .ToListAsync(cancellationToken);

        // Group by MainGroup → ItemGroup
        var result = items
            .Where(x => x.Group != null && x.Group.MainGroup != null)
            .GroupBy(x => x.Group.MainGroup) // group by MainGroup
            .Select(mainGroup =>
            {
                var itemGroups = mainGroup
                    .GroupBy(i => i.Group) // then group by ItemGroup
                    .Select(g =>
                    {
                        var groupItems = g.ToList();

                        var itemLimits = groupItems.Select(item =>
                        {
                            var received = receipts.Where(r => r.ItemId == item.Id).Sum(r => r.Quantity);
                            var issued = issues.Where(i => i.ItemId == item.Id).Sum(i => i.Quantity);
                            var balance = item.OpeningBalance + received - issued;

                            return new ItemLimitsResponseV2
                            {
                                ItemId = item.Id,
                                ItemName = item.NameAr,
                                OrderLimit = item.OrderLimit,
                                Balance = balance
                            };
                        }).ToList();

                        return new ItemsOrderLimitResponseV2
                        {
                            ItemGroupId = g.Key.Id,
                            ItemGroupName = g.Key.Name,
                            Items = itemLimits
                        };
                    })
                    .ToList();

                return new MainGroupResponseV2
                {
                    MainGroupId = mainGroup.Key.Id,
                    MainGroupName = mainGroup.Key.Name,
                    StoreId = store.Id,
                    StoreName = store.Name,
                    ItemGroups = itemGroups
                };
            })
            .ToList();

        return ErrorResponseModel<List<MainGroupResponseV2>>.Success(GenericErrors.GetSuccess, result);
    }
}


