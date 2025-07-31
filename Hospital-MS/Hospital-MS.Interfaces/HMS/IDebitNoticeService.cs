using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.DailyRestrictions;
using Hospital_MS.Core.Contracts.DebitNotices;
using System.Data;

namespace Hospital_MS.Interfaces.HMS;
public interface IDebitNoticeService
{
    Task<ErrorResponseModel<PartialDailyRestrictionResponse>> CreateAsync(DebitNoticeRequest request, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<DebitNoticeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, DebitNoticeRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
