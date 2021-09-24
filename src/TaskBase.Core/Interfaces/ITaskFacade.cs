using CoreTask = TaskBase.Core.TaskAggregate.Task;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.Linq;
using System;
using TaskBase.Core.TaskAggregate;

namespace TaskBase.Core.Interfaces
{
    public interface ITaskFacade
    {
        Task<CoreTask> CreateTaskAsync(string title, string description, DateTime dueDate, Guid userId, string userName, CancellationToken cancellationToken);
        Task<bool> SetTaskAsInProgressAsync(Guid taskId, CancellationToken cancellationToken);
        Task<bool> CloseTaskAsync(Guid taskId, CancellationToken cancellationToken);
        Task<bool> DeleteTaskAsync(Guid taskId, CancellationToken cancellationToken);
        Task<CoreTask> GetTaskDetailsAsync(Guid taskId);
        Task<IEnumerable<CoreTask>> GetTasksByUserIdAsync(Guid userId);
        Task<Core.TaskAggregate.User> GetUserByNameAsync(string userName);
    }
}
