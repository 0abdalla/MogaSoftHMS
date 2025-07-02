using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.StoreTypes;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS;

public class StoreTypeService(IUnitOfWork unitOfWork) : IStoreTypeService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorResponseModel<string>> CreateAsync(StoreTypeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var storeType = new StoreType
            {
                Name = request.Name,
                IsActive = true
            };

            await _unitOfWork.Repository<StoreType>().AddAsync(storeType, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, storeType.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<StoreTypeResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<StoreType>()
                .GetAll()
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
                query = query.Where(x => x.Name.Contains(filter.SearchText));

            var total = await query.CountAsync(cancellationToken);

            var list = await query
                .OrderByDescending(x => x.Id)
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new StoreTypeResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync(cancellationToken);

            return PagedResponseModel<List<StoreTypeResponse>>.Success(GenericErrors.GetSuccess, total, list);
        }
        catch (Exception)
        {
            return PagedResponseModel<List<StoreTypeResponse>>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<StoreTypeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var storeType = await _unitOfWork.Repository<StoreType>()
                .GetAll()
                .Where(x => x.Id == id && x.IsActive)
                .Select(x => new StoreTypeResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (storeType == null)
                return ErrorResponseModel<StoreTypeResponse>.Failure(GenericErrors.NotFound);

            return ErrorResponseModel<StoreTypeResponse>.Success(GenericErrors.GetSuccess, storeType);
        }
        catch (Exception)
        {
            return ErrorResponseModel<StoreTypeResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, StoreTypeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var storeType = await _unitOfWork.Repository<StoreType>()
                .GetByIdAsync(id, cancellationToken);

            if (storeType == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            storeType.Name = request.Name;

            _unitOfWork.Repository<StoreType>().Update(storeType);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, storeType.Id.ToString());
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
            var storeType = await _unitOfWork.Repository<StoreType>()
                .GetByIdAsync(id, cancellationToken);

            if (storeType == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            storeType.IsActive = false;

            _unitOfWork.Repository<StoreType>().Update(storeType);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, storeType.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }
}