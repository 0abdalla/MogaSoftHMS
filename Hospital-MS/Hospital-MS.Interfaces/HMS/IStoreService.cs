using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Stores;

namespace Hospital_MS.Interfaces.HMS;
public interface IStoreService
{
    Task<ErrorResponseModel<string>> CreateAsync(CreateStoreRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<StoreResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<StoreResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, CreateStoreRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<ErrorResponseModel<List<StoreMovementResponse>>> GetStoreMovementsAsync(int storeId, GetStoresMovementsRequest request, CancellationToken cancellationToken = default);
}
