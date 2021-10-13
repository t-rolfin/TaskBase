using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskBase.SignalRHub.Models
{
    public record NotificationModel(Guid Id, string Title, string Description);
}
