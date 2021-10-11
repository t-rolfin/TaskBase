using CoreTask = TaskBase.Core.TaskAggregate.Task;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.Linq;
using System;
using TaskBase.Core.TaskAggregate;
using TaskBase.Core.Enums;
using TaskBase.Core.NotificationAggregate;
using Rolfin.Result;

namespace TaskBase.Core.Interfaces
{
    public interface IFacade
    {
        Task<Result<CoreTask>> CreateTaskAsync(string title, string description, DateTime dueDate, Guid userId, string userName, CancellationToken cancellationToken);
        Task<Result<bool>> ChangeTaskState(Guid taskId, TaskState taskState, CancellationToken cancellationToken);
        Task<Result<bool>> DeleteTaskAsync(Guid taskId, CancellationToken cancellationToken);
        Task<CoreTask> GetTaskDetailsAsync(Guid taskId);
        Task<IEnumerable<CoreTask>> GetTasksByUserIdAsync(Guid userId);
        Task<Core.TaskAggregate.User> GetUserByNameAsync(string userName);
        Task<Result<bool>> EditDescriptionAsync(string taskId, string newDescription, CancellationToken cancellationToken);
        Task<Result<bool>> EditTitleAsync(string taskId, string newTitle, CancellationToken cancellationToken);
        Task<Result<Note>> CreateNoteAsync(string taskId, string noteContent, CancellationToken cancellationToken);
        Task<Result<bool>> EditNoteAsync(string taskId, string noteId, string newContent, CancellationToken cancellationToken);
        Task<IEnumerable<Note>> GetTaskNotesAsync(string taskId, CancellationToken cancellationToken);
        Task<Result<bool>> EliminateNoteFromTaskAsync(string taskId, string noteId, CancellationToken cancellationToken);

        Task<Result<Notification>> CreateNotificationAsync(Guid userId, string title, string description, CancellationToken cancellationToken);
        Task<Result<bool>> RemoveNotification(Guid notificationId, CancellationToken cancellationToken);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId, CancellationToken cancellationToken);
    }
}
