using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Components.Models
{
    public record PushNotificationModel(string Title, string Description, string UserId, bool isSuccess);
}
