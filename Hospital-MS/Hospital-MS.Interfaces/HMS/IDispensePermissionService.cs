using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.DailyRestrictions;
using Hospital_MS.Core.Contracts.DispensePermission;

namespace Hospital_MS.Interfaces.HMS;
public interface IDispensePermissionService
{
    Task<ErrorResponseModel<PartialDailyRestrictionResponse>> CreateAsync(DispensePermissionRequest request, CancellationToken cancellationToken = default);
    // Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<IReadOnlyList<DispensePermissionResponse>>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<DispensePermissionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, DispensePermissionRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}