using Hospital_MS.Core.Models;

namespace Hospital_MS.Interfaces.Common;
public interface INotificationPublisher
{
    Task PublishNotificationAsync(Notification notification);
}
