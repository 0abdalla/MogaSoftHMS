using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.PurchaseOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS;
public interface IPurchaseOrderService
{
    Task<ErrorResponseModel<string>> CreateAsync(PurchaseOrderRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, PurchaseOrderRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<PurchaseOrderResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<PurchaseOrderResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
