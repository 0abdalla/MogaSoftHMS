using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.MaterialIssuePermission;

namespace Hospital_MS.Interfaces.HMS;
public interface IMaterialIssuePermissionService
{
    Task<ErrorResponseModel<MaterialIssuePermissionToReturnResponse>> CreateAsync(MaterialIssuePermissionRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, MaterialIssuePermissionRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<MaterialIssuePermissionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<MaterialIssuePermissionResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default);
}
