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

        var fromDate = request.fromDate;
        var toDate = request.toDate;

        var receiptItemIds = await _unitOfWork.Repository<ReceiptPermissionItem>()
                        .GetAll(x => x.ReceiptPermission.StoreId == storeId && x.ReceiptPermission.PermissionDate >= fromDate && x.ReceiptPermission.PermissionDate <= toDate)
                        .Select(x => x.ItemId)
                        .Distinct()
                        .ToListAsync(cancellationToken);

        var issueItemIds = await _unitOfWork.Repository<MaterialIssueItem>()
                        .GetAll(x => x.MaterialIssuePermission.StoreId == storeId
                                  && x.MaterialIssuePermission.PermissionDate >= fromDate
                                  && x.MaterialIssuePermission.PermissionDate <= toDate)
                        .Select(x => x.ItemId)
                        .Distinct()
                        .ToListAsync(cancellationToken);

        var allItemIds = receiptItemIds.Union(issueItemIds).Distinct().ToList();

        var itemsQuery = _unitOfWork.Repository<Item>()
                    .GetAll(x => allItemIds.Contains(x.Id) && x.IsActive)
                    .Include(x => x.Group)
                    .ThenInclude(z => z.MainGroup).AsQueryable();


        if (request.MainGroupId.HasValue)
            itemsQuery = itemsQuery.Where(x => x.Group.MainGroupId == request.MainGroupId.Value);

        if (request.ItemGroupId.HasValue)
            itemsQuery = itemsQuery.Where(x => x.GroupId == request.ItemGroupId.Value);

        var items = await itemsQuery.ToListAsync(cancellationToken);

        // Get all ReceiptPermissions for this store and date rang
        var receiptDetails = await _unitOfWork.Repository<ReceiptPermissionItem>()
            .GetAll(x => x.ReceiptPermission.StoreId == storeId)
            .Include(x => x.ReceiptPermission)
            .Where(x => x.ReceiptPermission.PermissionDate >= fromDate && x.ReceiptPermission.PermissionDate <= toDate)
            .ToListAsync(cancellationToken);

        // Get all MatrialIssues for this store and date range
        var issueDetails = await _unitOfWork.Repository<MaterialIssueItem>()
            .GetAll(x => x.MaterialIssuePermission.StoreId == storeId)
            .Include(x => x.MaterialIssuePermission)
            .Where(x => x.MaterialIssuePermission.PermissionDate >= fromDate && x.MaterialIssuePermission.PermissionDate <= toDate)
            .ToListAsync(cancellationToken);

        // Get opening balances (before fromDate)
        var openingReceipts = await _unitOfWork.Repository<ReceiptPermissionItem>()
            .GetAll(x => x.ReceiptPermission.StoreId == storeId && x.ReceiptPermission.PermissionDate < fromDate)
            .Include(x => x.ReceiptPermission)
            .ToListAsync(cancellationToken);

        var openingIssues = await _unitOfWork.Repository<MaterialIssueItem>()
            .GetAll(x => x.MaterialIssuePermission.StoreId == storeId && x.MaterialIssuePermission.PermissionDate < fromDate)
            .Include(x => x.MaterialIssuePermission)
            .ToListAsync(cancellationToken);

        var itemMovements = items.Select(item =>
        {
            var openingBalance = openingReceipts.Where(r => r.ItemId == item.Id).Sum(r => r.Quantity)
                                - openingIssues.Where(i => i.ItemId == item.Id).Sum(i => i.Quantity);

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

        List<StoreMovementResponse> result = [];

        if (request.MainGroupId.HasValue)
        {
            var mainGroups = items.GroupBy(x => x.Group?.MainGroup)
                .Select(mainGroup => new StoreMovementResponse
                {
                    MainGroupId = mainGroup.Key?.Id,
                    MainGroupName = mainGroup.Key?.Name,
                    ItemGroups = mainGroup.GroupBy(x => x.Group)
                        .Select(itemGroup => new ItemGroupsResponse
                        {
                            ItemGroupId = itemGroup.Key?.Id,
                            ItemGroupName = itemGroup.Key?.Name,
                            Items = itemMovements.Where(m => itemGroup.Any(i => i.Id == m.ItemId)).ToList()
                        }).ToList()

                }).ToList();

            result.AddRange(mainGroups);
        }
        else if (request.ItemGroupId.HasValue)
        {
            var itemGroups = items.GroupBy(x => x.Group)
                .Select(itemGroup => new StoreMovementResponse
                {
                    MainGroupId = null,
                    MainGroupName = null,
                    ItemGroups = new List<ItemGroupsResponse>
                    {
                        new ItemGroupsResponse
                        {
                            ItemGroupId = itemGroup.Key?.Id,
                            ItemGroupName = itemGroup.Key?.Name,
                            Items = itemMovements.Where(m => itemGroup.Any(i => i.Id == m.ItemId)).ToList()
                        }
                    }
                }).ToList();

            result.AddRange(itemGroups);
        }
        else
        {
            result.Add(new StoreMovementResponse
            {
                MainGroupId = null,
                MainGroupName = null,
                ItemGroups = new List<ItemGroupsResponse>
                {
                    new ItemGroupsResponse
                    {
                        ItemGroupId = null,
                        ItemGroupName = null,
                        Items = itemMovements
                    }
                }
            });
        }


        return ErrorResponseModel<List<StoreMovementResponse>>.Success(GenericErrors.GetSuccess, result);
    }
}

