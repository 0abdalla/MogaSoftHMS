using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.PurchaseRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS;
public interface IPurchaseRequestService
{
    Task<ErrorResponseModel<string>> CreateAsync(PurchaseRequestRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, PurchaseRequestRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<PurchaseRequestResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<PurchaseRequestResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}