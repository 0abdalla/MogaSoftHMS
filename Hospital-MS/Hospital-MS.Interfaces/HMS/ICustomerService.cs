using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Customers;
using Hospital_MS.Core.Contracts.PurchaseOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS;
public interface ICustomerService
{
    Task<ErrorResponseModel<string>> CreateAsync(CustomerRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, CustomerRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<CustomerResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<CustomerResponse>>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
