using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Stores;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
}

