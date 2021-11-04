using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Services;
using TaskBase.Data.Identity;

namespace TaskBase.Data.NotificationService
{
    public class NotificationService : INotificationService
    {
        private HubConnection _hubConnection;
        readonly IConfiguration _configuration;
        readonly ILogger<NotificationService> _logger;
        readonly IAuthTokenFactory _tokenFactory;

        public NotificationService(IConfiguration configuration,
            ILogger<NotificationService> logger,
            IAuthTokenFactory tokenFactory
            )
        {
            _configuration = configuration;
            _logger = logger;
            _tokenFactory = tokenFactory;
        }

        public async Task Notify(Guid userId, bool isSuccess, string message, CancellationToken cancellationToken)
        {
            try
            {
                if (!bool.TryParse(_configuration["SignalR:IsEnabled"], out bool isEnabled) || !isEnabled)
                    return;

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

        public async Task PushNotification(Guid id, string userId, bool isSuccess, string description, string title, CancellationToken cancellationToken)
        {
            try
            {
                if (!bool.TryParse(_configuration["SignalR:IsEnabled"], out bool isEnabled) || !isEnabled)
                    return;

                await CreateHubConnection(userId);
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

        async Task CreateHubConnection(string userId)
        {
            var accessToken = await _tokenFactory.GetTokenAsync(userId);

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(new Uri(_configuration["SignalR:HubUrl"]), x =>
                {
                    x.AccessTokenProvider = () => Task.FromResult(accessToken);
                })
                .Build();

            await _hubConnection.StartAsync();
        }

        public void Dispose()
        {
            _hubConnection?.StopAsync();
            _hubConnection?.DisposeAsync();
        }
    }
}
