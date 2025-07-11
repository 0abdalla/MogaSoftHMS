using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.RestrictionTypes;

namespace Hospital_MS.Interfaces.HMS;

public interface IRestrictionTypeService
{
    Task<ErrorResponseModel<string>> CreateAsync(RestrictionTypeRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, RestrictionTypeRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<RestrictionTypeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<RestrictionTypeResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default);
}