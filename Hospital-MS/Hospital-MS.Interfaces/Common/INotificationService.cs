using Hospital_MS.Core.Models;

namespace Hospital_MS.Interfaces.Common;
public interface INotificationService
{
    Task SendNewPurchaseRequestNotification(int purchaseId);
    Task CreateAndNotifyAsync(Notification notification, CancellationToken cancellationToken = default);
}
