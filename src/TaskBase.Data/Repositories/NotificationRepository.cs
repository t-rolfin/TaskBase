using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Core.Interfaces;
using TaskBase.Core.NotificationAggregate;
using TaskBase.Core.TaskAggregate;
using TaskBase.Data.Exceptions;

namespace TaskBase.Data.Repositories
{
    internal class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(TaskContext _context) : base(_context) { }

        public async Task<Notification> AddAsync(Notification entity, CancellationToken cancellationToken = default)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public async System.Threading.Tasks.Task Remove(Notification entity, CancellationToken cancellationToken = default)
        {
            dbSet.Remove(entity);
            await System.Threading.Tasks.Task.CompletedTask;
        }

        public Task<Notification> UpdateAsync(Notification entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Notification> GetNotificationAsync(Guid id, CancellationToken cancellationToken)
        {
            var notification = await dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (notification == default)
                throw new NotificationNotFoundException($"The notification with Id: {id} wasn't found.");

            return notification;
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId, CancellationToken cancellationToken)
        {
            var notifications = dbSet.Where(x => x.UserId == userId);
            return await System.Threading.Tasks.Task.FromResult(notifications);
        }
    }
}
