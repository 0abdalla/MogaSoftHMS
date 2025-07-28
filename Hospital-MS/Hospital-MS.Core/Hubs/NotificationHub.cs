using Hospital_MS.Core.Common.Consts;
using Microsoft.AspNetCore.SignalR;

namespace Hospital_MS.Core.Hubs;

public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var user = Context.User;

        if (user?.IsInRole(DefaultRoles.SystemAdmin.Name) == true)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SystemAdmins");
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SystemAdmins");
        await base.OnDisconnectedAsync(exception);
    }
}
