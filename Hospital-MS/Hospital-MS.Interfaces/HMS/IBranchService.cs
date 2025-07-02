using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Branches;

namespace Hospital_MS.Interfaces.HMS;

public interface IBranchService
{
    Task<ErrorResponseModel<string>> CreateAsync(BranchRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, BranchRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<BranchResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<BranchResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default);
}