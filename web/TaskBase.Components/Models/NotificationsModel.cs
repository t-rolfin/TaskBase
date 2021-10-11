using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Components.Models
{
    public record NotificationsModel(List<NotificationModel> Notifications, int NotificationCount);
    public record NotificationModel(Guid Id, string Title, string Description);
}
