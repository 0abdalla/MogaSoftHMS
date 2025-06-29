using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.PurchasePermission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS;
public interface IReceiptPermissionService
{
    Task<ErrorResponseModel<string>> CreateAsync(ReceiptPermissionRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, ReceiptPermissionRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<ReceiptPermissionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<ReceiptPermissionResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
