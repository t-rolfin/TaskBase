using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        readonly IConfiguration _configuration;
        readonly ILogger<NotificationService> _logger;

        public NotificationService(IConfiguration configuration, ILogger<NotificationService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        async Task CreateHubConnection(string groupName)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(new Uri(_configuration["SignalR:Url"]), x =>
                {
                    x.AccessTokenProvider = () => Task.FromResult(groupName);
                }).Build();

            await _hubConnection.StartAsync();

            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _hubConnection.StopAsync();
            _hubConnection.DisposeAsync();
        }
    }
}
