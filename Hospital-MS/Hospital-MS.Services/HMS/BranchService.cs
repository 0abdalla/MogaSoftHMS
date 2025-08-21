using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Branches;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS;

public class BranchService : IBranchService
{
    private readonly IUnitOfWork _unitOfWork;

    public BranchService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorResponseModel<string>> CreateAsync(BranchRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var branch = new Branch
            {
                Name = request.Name,
                Location = request.Location,
                ContactNumber = request.ContactNumber,
                Email = request.Email,
                IsActive = true
            };

            await _unitOfWork.Repository<Branch>().AddAsync(branch, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, branch.Id.ToString());
        }
        catch
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, BranchRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(id, cancellationToken);
            if (branch == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            branch.Name = request.Name;
            branch.Location = request.Location;
            branch.ContactNumber = request.ContactNumber;
            branch.Email = request.Email;

            _unitOfWork.Repository<Branch>().Update(branch);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, branch.Id.ToString());
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
            var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(id, cancellationToken);
            if (branch == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            branch.IsActive = false;
            _unitOfWork.Repository<Branch>().Update(branch);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, branch.Id.ToString());
        }
        catch
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<BranchResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var branch = await _unitOfWork.Repository<Branch>()
                .GetAll()
                .Where(x => x.Id == id && x.IsActive)
                .Select(x => new BranchResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Location = x.Location,
                    ContactNumber = x.ContactNumber,
                    Email = x.Email,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (branch == null)
                return ErrorResponseModel<BranchResponse>.Failure(GenericErrors.NotFound);

            return ErrorResponseModel<BranchResponse>.Success(GenericErrors.GetSuccess, branch);
        }
        catch
        {
            return ErrorResponseModel<BranchResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<BranchResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<Branch>()
                .GetAll()
                .OrderByDescending(x => x.Id)
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
                query = query.Where(x => x.Name.Contains(filter.SearchText));

            var total = await query.CountAsync(cancellationToken);

            var list = await query
                .OrderByDescending(x => x.Id)
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new BranchResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Location = x.Location,
                    ContactNumber = x.ContactNumber,
                    Email = x.Email,
                    IsActive = x.IsActive
                })
                .ToListAsync(cancellationToken);

            return PagedResponseModel<List<BranchResponse>>.Success(GenericErrors.GetSuccess, total, list);
        }
        catch
        {
            return PagedResponseModel<List<BranchResponse>>.Failure(GenericErrors.TransFailed);
        }
    }
}