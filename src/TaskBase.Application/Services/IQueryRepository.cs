using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Models;
using TaskBase.Core.NotificationAggregate;

namespace TaskBase.Application.Services
{
    public interface IQueryRepository
    {
        Task<TaskModel> GetTaskAsync(Guid taskId);
        Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(Guid userId);
        Task<IEnumerable<NoteModel>> GetTaskNotesAsync(string taskId, CancellationToken cancellationToken);

        Task<IEnumerable<NotificationModel>> GetUserNotificationsAsync(Guid userId, CancellationToken cancellationToken);
    }
}
