using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.RestrictionTypes;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS;

public class RestrictionTypeService(IUnitOfWork unitOfWork) : IRestrictionTypeService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorResponseModel<string>> CreateAsync(RestrictionTypeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = new RestrictionType
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = true
            };
            await _unitOfWork.Repository<RestrictionType>().AddAsync(entity, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, entity.Id.ToString());
        }
        catch
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, RestrictionTypeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<RestrictionType>().GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            entity.Name = request.Name;
            entity.Description = request.Description;

            _unitOfWork.Repository<RestrictionType>().Update(entity);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, entity.Id.ToString());
        }
        catch
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<RestrictionType>().GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            entity.IsActive = false;
            _unitOfWork.Repository<RestrictionType>().Update(entity);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, entity.Id.ToString());
        }
        catch
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<RestrictionTypeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<RestrictionType>()
                .GetAll()
                .Where(x => x.Id == id && x.IsActive)
                .Select(x => new RestrictionTypeResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
                return ErrorResponseModel<RestrictionTypeResponse>.Failure(GenericErrors.NotFound);

            return ErrorResponseModel<RestrictionTypeResponse>.Success(GenericErrors.GetSuccess, entity);
        }
        catch
        {
            return ErrorResponseModel<RestrictionTypeResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<RestrictionTypeResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<RestrictionType>()
                .GetAll()
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
                query = query.Where(x => x.Name.Contains(filter.SearchText));

            var total = await query.CountAsync(cancellationToken);

            var list = await query
                .OrderByDescending(x => x.Id)
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new RestrictionTypeResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToListAsync(cancellationToken);

            return PagedResponseModel<List<RestrictionTypeResponse>>.Success(GenericErrors.GetSuccess, total, list);
        }
        catch
        {
            return PagedResponseModel<List<RestrictionTypeResponse>>.Failure(GenericErrors.TransFailed);
        }
    }
}