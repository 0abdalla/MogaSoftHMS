using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.ItemGroups;

namespace Hospital_MS.Interfaces.HMS;

public interface IItemGroupService
{
    Task<ErrorResponseModel<string>> CreateAsync(ItemGroupRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, ItemGroupRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<ItemGroupResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<ItemGroupResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default);
}