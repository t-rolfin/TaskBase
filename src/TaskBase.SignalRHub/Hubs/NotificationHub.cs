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
            var ss = Context.Features.Get<IHttpTransportFeature>().TransportType.ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Claims.First().Value);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.Claims.First().Value);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(NotificationModel notification)
        {
            await Clients.Group(Context.User.Claims.First().Value)
                .SendAsync("ReceiveNotification", notification);
        }
    }
}
