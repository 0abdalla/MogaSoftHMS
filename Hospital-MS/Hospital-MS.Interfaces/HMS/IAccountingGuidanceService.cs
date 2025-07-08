using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.AccountingGuidance;

namespace Hospital_MS.Interfaces.HMS;

public interface IAccountingGuidanceService
{
    Task<ErrorResponseModel<string>> CreateAsync(AccountingGuidanceRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, AccountingGuidanceRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<AccountingGuidanceResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<AccountingGuidanceResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default);
}