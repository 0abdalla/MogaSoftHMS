using Hospital_MS.Interfaces.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers;

[Authorize]
public class NotificationsController(INotificationService notificationService) : ApiBaseController
{
    private readonly INotificationService _notificationService = notificationService;

    [HttpGet("")]
    public async Task<IActionResult> GetAllNotifications(CancellationToken cancellationToken)
    {
        var notifications = await _notificationService.GetAllNotificationsAsync(cancellationToken);
        return Ok(notifications);
    }

    [HttpPut("read/{id}")]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken cancellationToken)
    {
        await _notificationService.MarkNotificationAsReadAsync(id, cancellationToken);
        return Ok();
    }
}
