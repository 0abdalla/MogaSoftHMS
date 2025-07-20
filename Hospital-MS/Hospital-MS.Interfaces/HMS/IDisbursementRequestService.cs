using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Disbursement;

namespace Hospital_MS.Interfaces.HMS;
public interface IDisbursementRequestService
{
    Task<ErrorResponseModel<DisbursementToReturnResponse>> CreateAsync(DisbursementReq request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, DisbursementReq request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<DisbursementResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<DisbursementResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default);

    Task<ErrorResponseModel<string>> ApproveDisbursementRequestAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<DisbursementResponse>>> GetAllApprovedAsync(CancellationToken cancellationToken = default);

}
