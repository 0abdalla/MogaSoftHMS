using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.ItemGroups;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS;

public class ItemGroupService(IUnitOfWork unitOfWork) : IItemGroupService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorResponseModel<string>> CreateAsync(ItemGroupRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = new ItemGroup
            {
                Name = request.Name,
                MainGroupId = request.MainGroupId,
                IsActive = true
            };
            await _unitOfWork.Repository<ItemGroup>().AddAsync(entity, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, entity.Id.ToString());
        }
        catch
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, ItemGroupRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<ItemGroup>().GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            entity.Name = request.Name;
            entity.MainGroupId = request.MainGroupId;

            _unitOfWork.Repository<ItemGroup>().Update(entity);
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
            var entity = await _unitOfWork.Repository<ItemGroup>().GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            entity.IsActive = false;
            _unitOfWork.Repository<ItemGroup>().Update(entity);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, entity.Id.ToString());
        }
        catch
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<ItemGroupResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<ItemGroup>()
                .GetAll(x=>x.IsActive && x.Id == id)
                .Include(x => x.MainGroup)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy) 
                .Select(x => new ItemGroupResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    MainGroupId = x.MainGroupId,
                    MainGroupName = x.MainGroup != null ? x.MainGroup.Name : null,
                    Audit = new AuditResponse
                    {
                        CreatedBy = x.CreatedBy != null ? x.CreatedBy.UserName : null,
                        CreatedOn = x.CreatedOn,
                        UpdatedBy = x.UpdatedBy != null ? x.UpdatedBy.UserName : null,
                        UpdatedOn = x.UpdatedOn
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
                return ErrorResponseModel<ItemGroupResponse>.Failure(GenericErrors.NotFound);

            return ErrorResponseModel<ItemGroupResponse>.Success(GenericErrors.GetSuccess, entity);
        }
        catch
        {
            return ErrorResponseModel<ItemGroupResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<ItemGroupResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<ItemGroup>()
                .GetAll(x => x.IsActive)
                .Include(x => x.MainGroup).AsQueryable();
                

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
                query = query.Where(x => x.Name.Contains(filter.SearchText));

            var total = await query.CountAsync(cancellationToken);

            var list = await query
                .OrderByDescending(x => x.Id)
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new ItemGroupResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    MainGroupId = x.MainGroupId,
                    MainGroupName = x.MainGroup != null ? x.MainGroup.Name : null
                })
                .ToListAsync(cancellationToken);

            return PagedResponseModel<List<ItemGroupResponse>>.Success(GenericErrors.GetSuccess, total, list);
        }
        catch
        {
            return PagedResponseModel<List<ItemGroupResponse>>.Failure(GenericErrors.TransFailed);
        }
    }
}