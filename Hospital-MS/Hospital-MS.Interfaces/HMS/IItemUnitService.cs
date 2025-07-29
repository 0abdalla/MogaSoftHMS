using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.ItemUnits;

namespace Hospital_MS.Interfaces.HMS;
public interface IItemUnitService
{
    Task<ErrorResponseModel<string>> CreateAsync(ItemUnitRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, ItemUnitRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<ItemUnitResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<ItemUnitResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default);
}
