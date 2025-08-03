using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.DailyRestrictions;

namespace Hospital_MS.Interfaces.HMS;
public interface IDailyRestrictionService
{
    Task<ErrorResponseModel<string>> CreateAsync(DailyRestrictionRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, DailyRestrictionRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<DailyRestrictionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<DailyRestrictionResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default);
    Task<string> GenerateRestrictionNumberAsync(CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<List<AccountReportResponse>>> GetAccountReportAsync(int accountId, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default);
}