using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.SignalRHub.Models;

namespace TaskBase.SignalRHub.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Claims.First().Value);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.Claims.First().Value);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task PersistenceNotification(NotificationModel notification)
        {
            await Clients.OthersInGroup(Context.User.Claims.First().Value)
                .SendAsync("ReceiveNotification", notification);
        }

        public async Task PageNotification(PageNotificationModel pageNotification)
        {
            await Clients.Group(pageNotification.UserId.ToString())
                .SendAsync("RecievePageNotification", pageNotification);
        }
    }
}
