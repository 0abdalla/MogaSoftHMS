using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.FiscalYears;
using Hospital_MS.Core.Contracts.MainGroups;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS;
public class MainGroupService(IUnitOfWork unitOfWork) : IMainGroupService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorResponseModel<string>> CreateAsync(MainGroupRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var mainGroup = new MainGroup
            {
                Name = request.Name
            };

            await _unitOfWork.Repository<MainGroup>().AddAsync(mainGroup, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, mainGroup.Id.ToString());
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
            var mainGroup = await _unitOfWork.Repository<MainGroup>()
                .GetAll(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (mainGroup == null)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }

            mainGroup.IsActive = false;

            _unitOfWork.Repository<MainGroup>().Update(mainGroup);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);


        }
    }

    public async Task<PagedResponseModel<List<MainGroupResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<MainGroup>()
                .GetAll()
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                query = query.Where(x => x.Name.Contains(filter.SearchText));
            }

            var total = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new MainGroupResponse
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync(cancellationToken);

            return PagedResponseModel<List<MainGroupResponse>>.Success(GenericErrors.GetSuccess, total, items);
        }
        catch (Exception)
        {
            return PagedResponseModel<List<MainGroupResponse>>.Failure(GenericErrors.TransFailed);
        }

    }

    public async Task<ErrorResponseModel<MainGroupResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {

        try
        {
            var mainGroup = await _unitOfWork.Repository<MainGroup>()
                .GetAll(x => x.Id == id && x.IsActive)
                .Include(x=> x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(cancellationToken);

            if (mainGroup == null)
            {
                return ErrorResponseModel<MainGroupResponse>.Failure(GenericErrors.NotFound);
            }

            var response = new MainGroupResponse
            {
                Id = mainGroup.Id,
                Name = mainGroup.Name,
                Audit = new AuditResponse
                {
                    CreatedBy = mainGroup.CreatedBy?.UserName ?? "",
                    CreatedOn = mainGroup.CreatedOn,
                    UpdatedBy = mainGroup.UpdatedBy?.UserName ?? "",
                    UpdatedOn = mainGroup.UpdatedOn
                }
            };

            return ErrorResponseModel<MainGroupResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<MainGroupResponse>.Failure(GenericErrors.TransFailed);

        }

    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, MainGroupRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var mainGroup = await _unitOfWork.Repository<MainGroup>()
                .GetAll(x => x.Id == id && x.IsActive)
                .FirstOrDefaultAsync(cancellationToken);

            if (mainGroup == null)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }

            mainGroup.Name = request.Name;

            _unitOfWork.Repository<MainGroup>().Update(mainGroup);

            await _unitOfWork.CompleteAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }
}
