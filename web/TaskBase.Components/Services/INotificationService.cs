using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;

namespace TaskBase.Components.Services
{
    public interface INotificationService
    {
        Task<NotificationsModel> GetNotificationsAsync(Guid userId);
        Task<bool> DeleteNotification(Guid notificationId);
        Task PushNotification(PushNotificationModel model);
        Task PageNotification(PageNotificationModel model);
    }
}
