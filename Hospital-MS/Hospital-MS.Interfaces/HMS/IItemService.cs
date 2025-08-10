using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Items;

namespace Hospital_MS.Interfaces.HMS;
public interface IItemService
{
    Task<ErrorResponseModel<string>> CreateItemAsync(ItemRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateItemAsync(int id, ItemRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<ItemResponse>> GetItemByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<ItemResponse>>> GetItemsAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteItemAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<ItemMovementResult>> GetItemMovementAsync(int id, GetItemMovementRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<ItemMovementResult>> GetItemMovementAsyncV2(int id, GetItemMovementRequest request, CancellationToken cancellationToken = default);
}
