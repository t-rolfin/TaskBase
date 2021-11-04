using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Components.Utils;

namespace TaskBase.Components.Services
{
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _apiClient;

        public NotificationService(HttpClient httpClient)
        {
            _apiClient = httpClient;
        }

        public async Task<bool> DeleteNotification(Guid notificationId)
        {
            var response = await _apiClient.DeleteAsync($"/api/notification/{notificationId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<NotificationsModel> GetNotificationsAsync(Guid userId)
        {
            var response = await _apiClient.GetAsync($"/api/notifications/{userId}");

            if (!response.IsSuccessStatusCode)
                return new NotificationsModel(new List<NotificationModel>(), 0);

            var notifications = await HttpResponseParser
                .Parse<IEnumerable<NotificationModel>>(response.Content);

            var result = new NotificationsModel(
                    notifications.ToList(),
                    notifications.Count()
                );

            return result;
        }

        public async Task PushNotification(PushNotificationModel model)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json");

            var response = await _apiClient.PostAsync($"/api/notification", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task PageNotification(PageNotificationModel model)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json");

            var response = await _apiClient.PostAsync($"/api/notification/page", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
