using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.StoreTypes;

namespace Hospital_MS.Interfaces.HMS;

public interface IStoreTypeService
{
    Task<ErrorResponseModel<string>> CreateAsync(StoreTypeRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<StoreTypeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<StoreTypeResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, StoreTypeRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
