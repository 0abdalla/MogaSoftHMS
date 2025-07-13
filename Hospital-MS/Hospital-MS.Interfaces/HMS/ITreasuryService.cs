using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Treasuries;

namespace Hospital_MS.Interfaces.HMS;
public interface ITreasuryService
{
    Task<ErrorResponseModel<string>> CreateTreasuryAsync(TreasuryRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateTreasuryAsync(int id, TreasuryRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<TreasuryResponse>> GetTreasuryByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<TreasuryResponse>>> GetTreasuriesAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteTreasuryAsync(int id, CancellationToken cancellationToken = default);

    Task<ErrorResponseModel<List<TreasuryResponse>>> GetEnabledTreasuriesAsync(CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<List<TreasuryResponse>>> GetDisabledTreasuriesAsync(CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> EnableTreasuryAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DisableTreasuryAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> AssignTreasuryToStaffAsync(int staffId, int treasuryId, CancellationToken cancellationToken);
    Task<ErrorResponseModel<TreasuryTransactionResponse>> GetTreasuryTransactionsAsync(int treasuryId, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default);

}
