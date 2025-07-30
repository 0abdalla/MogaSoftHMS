using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.AdditionNotifications;
using Hospital_MS.Core.Contracts.DailyRestrictions;

namespace Hospital_MS.Interfaces.HMS;
public interface IAdditionNotificationService
{
    Task<ErrorResponseModel<PartialDailyRestrictionResponse>> CreateAsync(AdditionNotificationRequest request, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<AdditionNotificationResponse>>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<AdditionNotificationResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, AdditionNotificationRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}