using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS;
public interface ISupplierService
{
    Task<ErrorResponseModel<string>> CreateSupplierAsync(SupplierRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateSupplierAsync(int id, SupplierRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<SupplierResponse>> GetSupplierByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<SupplierResponse>>> GetAllSuppliersAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteSupplierAsync(int id, CancellationToken cancellationToken = default);
}
