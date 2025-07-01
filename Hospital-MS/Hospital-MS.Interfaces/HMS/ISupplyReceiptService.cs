using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.SupplyReceipts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS;
public interface ISupplyReceiptService
{
    Task<ErrorResponseModel<string>> CreateSupplyReceiptAsync(SupplyReceiptRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<SupplyReceiptResponse>> GetSupplyReceiptByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateSupplyReceiptAsync(int id, SupplyReceiptRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteSupplyReceiptAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
}