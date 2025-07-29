using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.ItemUnits;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS;
public class ItemUnitService(IUnitOfWork unitOfWork) : IItemUnitService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorResponseModel<string>> CreateAsync(ItemUnitRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingUnit = await _unitOfWork.Repository<ItemUnit>()
                .AnyAsync(x => (x.Name == request.Name) && x.IsActive,
                         cancellationToken);

            if (existingUnit)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            var itemUnit = new ItemUnit
            {
                Name = request.Name
            };

            await _unitOfWork.Repository<ItemUnit>().AddAsync(itemUnit, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, itemUnit.Id.ToString());
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
            var itemUnit = await _unitOfWork.Repository<ItemUnit>()
                .GetByIdAsync(id, cancellationToken);

            if (itemUnit == null || !itemUnit.IsActive)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            itemUnit.IsActive = false;

            _unitOfWork.Repository<ItemUnit>().Update(itemUnit);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, itemUnit.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<ItemUnitResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<ItemUnit>()
                .GetAll()
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Where(x => x.IsActive);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new ItemUnitResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Audit = new AuditResponse
                    {
                        CreatedBy = x.CreatedBy.UserName,
                        CreatedOn = x.CreatedOn,
                        UpdatedBy = x.UpdatedBy.UserName,
                        UpdatedOn = x.UpdatedOn
                    }
                })
                .ToListAsync(cancellationToken);

            return PagedResponseModel<List<ItemUnitResponse>>.Success(GenericErrors.GetSuccess, totalCount, items);
        }
        catch (Exception)
        {
            return PagedResponseModel<List<ItemUnitResponse>>.Failure(GenericErrors.TransFailed);
        }

    }

    public async Task<ErrorResponseModel<ItemUnitResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var itemUnit = await _unitOfWork.Repository<ItemUnit>()
                            .GetAll(x => x.Id == id)
                            .Include(x => x.CreatedBy)
                            .Include(x => x.UpdatedBy)
                            .FirstOrDefaultAsync(cancellationToken);

            if (itemUnit == null || !itemUnit.IsActive)
                return ErrorResponseModel<ItemUnitResponse>.Failure(GenericErrors.NotFound);

            var response = new ItemUnitResponse
            {
                Id = itemUnit.Id,
                Name = itemUnit.Name,
                Audit = new AuditResponse
                {
                    CreatedBy = itemUnit.CreatedBy?.UserName,
                    CreatedOn = itemUnit.CreatedOn,
                    UpdatedBy = itemUnit?.UpdatedBy?.UserName,
                    UpdatedOn = itemUnit?.UpdatedOn
                }
            };
            return ErrorResponseModel<ItemUnitResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<ItemUnitResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, ItemUnitRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var itemUnit = await _unitOfWork.Repository<ItemUnit>()
                .GetByIdAsync(id, cancellationToken);

            if (itemUnit == null || !itemUnit.IsActive)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            var existingUnit = await _unitOfWork.Repository<ItemUnit>()
                .AnyAsync(x => (x.Name == request.Name) && x.Id != id && x.IsActive, cancellationToken);
            if (existingUnit)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            itemUnit.Name = request.Name;

            _unitOfWork.Repository<ItemUnit>().Update(itemUnit);

            await _unitOfWork.CompleteAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, itemUnit.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }

    }
}
