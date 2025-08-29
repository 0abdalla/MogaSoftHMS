using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.PriceQuotation;

namespace Hospital_MS.Interfaces.HMS;
public interface IPriceQuotationService
{
    Task<ErrorResponseModel<string>> CreateAsync(PriceQuotationRequest request, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<PriceQuotationResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<PriceQuotationResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, PriceQuotationRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<PriceQuotationResponse>>> GetAllByPurchaseRequestIdAsync(int purchaseRequestId, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<string>> SubmitPriceQuotationByPurchaseRequestIdAsync(int purchaseRequestId, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<PriceQuotationResponse>>> GetAllApprovedAsync(CancellationToken cancellationToken = default);
    Task<PagedResponseModel<string>> SubmitPriceQuotationByPurchaseRequestIdAsyncV2(int purchaseRequestId, CancellationToken cancellationToken = default);
}
