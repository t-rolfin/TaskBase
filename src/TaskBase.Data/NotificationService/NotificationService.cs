using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Services;

namespace TaskBase.Data.NotificationService
{
    public class NotificationService : INotificationService
    {
        private HubConnection _hubConnection;

        public async Task Notify(Guid userId, bool isSuccess, string message, CancellationToken cancellationToken)
        {
            try
            {
                await CreateHubConnection(userId.ToString());

                await _hubConnection.InvokeAsync(
                    "PageNotification",
                    new PageNotificationModel(isSuccess, message, userId),
                    cancellationToken
                    );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task PersistentNotification(Guid id, Guid userId, bool isSuccess, string description, string title, CancellationToken cancellationToken)
        {
            try
            {
                await CreateHubConnection(userId.ToString());
                await _hubConnection.InvokeAsync(
                    "PersistenceNotification",
                    new NotificationModel(id, title, description),
                    cancellationToken
                    );
            }
            catch (Exception)
            {
                throw;
            }
        }

        async Task CreateHubConnection(string groupName)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(new Uri("https://localhost:5004/notificationhub"), x =>
                {
                    x.AccessTokenProvider = () => Task.FromResult(groupName);
                }).Build();

            await _hubConnection.StartAsync();

            await Task.CompletedTask;
        }
    }
}
