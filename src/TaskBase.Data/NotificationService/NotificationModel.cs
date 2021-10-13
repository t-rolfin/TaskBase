using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Data.NotificationService
{
    public record NotificationModel(Guid Id, string Title, string Description);
}
