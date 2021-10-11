using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskBase.Application.Services;

namespace TaskBase.Data.NotificationService
{
    public class NotificationService : INotificationService
    {
        private HubConnection _hubConnection;

        public async Task Notify(Guid userId, bool isSuccess, string message)
        {
            try
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl(new Uri("https://localhost:5004/notificationhub"), x =>
                    {
                        x.AccessTokenProvider = () => Task.FromResult(userId.ToString());
                    }).Build();

                await _hubConnection.StartAsync();

                await _hubConnection.InvokeAsync("PageNotification", new PageNotificationModel(isSuccess, message, userId));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
            }
        }
    }
}
