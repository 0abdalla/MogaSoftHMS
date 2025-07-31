using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.DailyRestrictions;
using Hospital_MS.Core.Contracts.SupplyReceipts;
using System.Data;

namespace Hospital_MS.Interfaces.HMS;
public interface ISupplyReceiptService
{
    Task<ErrorResponseModel<PartialDailyRestrictionResponse>> CreateSupplyReceiptAsync(SupplyReceiptRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<SupplyReceiptResponse>> GetSupplyReceiptByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateSupplyReceiptAsync(int id, SupplyReceiptRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteSupplyReceiptAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
}