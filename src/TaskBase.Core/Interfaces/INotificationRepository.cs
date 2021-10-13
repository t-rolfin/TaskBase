using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Core.NotificationAggregate;

namespace TaskBase.Core.Interfaces
{
    public interface INotificationRepository : IAsyncRepository<Notification, Guid>
    {
        Task<Notification> GetNotificationAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId, CancellationToken cancellationToken);
    }
}
