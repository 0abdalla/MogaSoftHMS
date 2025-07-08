using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.AccountingGuidance;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS;

public class AccountingGuidanceService(IUnitOfWork unitOfWork) : IAccountingGuidanceService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorResponseModel<string>> CreateAsync(AccountingGuidanceRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = new AccountingGuidance
            {
                Name = request.Name,
                IsActive = true
            };

            await _unitOfWork.Repository<AccountingGuidance>().AddAsync(entity, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, entity.Id.ToString());
        }
        catch
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, AccountingGuidanceRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<AccountingGuidance>().GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            entity.Name = request.Name;

            _unitOfWork.Repository<AccountingGuidance>().Update(entity);
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
            var entity = await _unitOfWork.Repository<AccountingGuidance>().GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            entity.IsActive = false;
            _unitOfWork.Repository<AccountingGuidance>().Update(entity);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, entity.Id.ToString());
        }
        catch
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<AccountingGuidanceResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<AccountingGuidance>()
                .GetAll()
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Where(x => x.Id == id && x.IsActive)
                .Select(x => new AccountingGuidanceResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    Audit = new()
                    {
                        CreatedBy = x.CreatedBy != null ? x.CreatedBy.FirstName + " " + x.CreatedBy.LastName : null,
                        CreatedOn = x.CreatedOn,
                        UpdatedBy = x.UpdatedBy != null ? x.UpdatedBy.FirstName + " " + x.UpdatedBy.LastName : null,
                        UpdatedOn = x.UpdatedOn
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
                return ErrorResponseModel<AccountingGuidanceResponse>.Failure(GenericErrors.NotFound);

            return ErrorResponseModel<AccountingGuidanceResponse>.Success(GenericErrors.GetSuccess, entity);
        }
        catch
        {
            return ErrorResponseModel<AccountingGuidanceResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<AccountingGuidanceResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<AccountingGuidance>()
                .GetAll()
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
                query = query.Where(x => x.Name.Contains(filter.SearchText));

            var total = await query.CountAsync(cancellationToken);

            var list = await query
                .OrderByDescending(x => x.Id)
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new AccountingGuidanceResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    Audit = new()
                    {
                        CreatedBy = x.CreatedBy != null ? x.CreatedBy.FirstName + " " + x.CreatedBy.LastName : null,
                        CreatedOn = x.CreatedOn,
                        UpdatedBy = x.UpdatedBy != null ? x.UpdatedBy.FirstName + " " + x.UpdatedBy.LastName : null,
                        UpdatedOn = x.UpdatedOn
                    }
                })
                .ToListAsync(cancellationToken);

            return PagedResponseModel<List<AccountingGuidanceResponse>>.Success(GenericErrors.GetSuccess, total, list);
        }
        catch
        {
            return PagedResponseModel<List<AccountingGuidanceResponse>>.Failure(GenericErrors.TransFailed);
        }
    }
}