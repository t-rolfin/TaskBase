using CoreTask = TaskBase.Core.TaskAggregate.Task;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.Linq;
using System;
using TaskBase.Core.TaskAggregate;
using TaskBase.Core.Enums;

namespace TaskBase.Core.Interfaces
{
    public interface ITaskFacade
    {
        Task<CoreTask> CreateTaskAsync(string title, string description, DateTime dueDate, Guid userId, string userName, CancellationToken cancellationToken);
        Task<bool> ChangeTaskState(Guid taskId, TaskState taskState, CancellationToken cancellationToken);
        Task<bool> DeleteTaskAsync(Guid taskId, CancellationToken cancellationToken);
        Task<CoreTask> GetTaskDetailsAsync(Guid taskId);
        Task<IEnumerable<CoreTask>> GetTasksByUserIdAsync(Guid userId);
        Task<Core.TaskAggregate.User> GetUserByNameAsync(string userName);
        Task<bool> EditDescription(string taskId, string newDescription, CancellationToken cancellationToken);
        Task<bool> EditTitle(string taskId, string newTitle, CancellationToken cancellationToken);
    }
}
