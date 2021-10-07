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

namespace TaskBase.Core.Interfaces
{
    public interface IFacade
    {
        Task<CoreTask> CreateTaskAsync(string title, string description, DateTime dueDate, Guid userId, string userName, CancellationToken cancellationToken);
        Task<bool> ChangeTaskState(Guid taskId, TaskState taskState, CancellationToken cancellationToken);
        Task<bool> DeleteTaskAsync(Guid taskId, CancellationToken cancellationToken);
        Task<CoreTask> GetTaskDetailsAsync(Guid taskId);
        Task<IEnumerable<CoreTask>> GetTasksByUserIdAsync(Guid userId);
        Task<Core.TaskAggregate.User> GetUserByNameAsync(string userName);
        Task<bool> EditDescriptionAsync(string taskId, string newDescription, CancellationToken cancellationToken);
        Task<bool> EditTitleAsync(string taskId, string newTitle, CancellationToken cancellationToken);
        Task<Note> CreateNoteAsync(string taskId, string noteContent, CancellationToken cancellationToken);
        Task<bool> EditNoteAsync(string taskId, string noteId, string newContent, CancellationToken cancellationToken);
        Task<IEnumerable<Note>> GetTaskNotesAsync(string taskId, CancellationToken cancellationToken);
        Task<bool> EliminateNoteFromTaskAsync(string taskId, string noteId, CancellationToken cancellationToken);

        Task<Notification> CreateNotificationAsync(Guid userId, string title, string description, CancellationToken cancellationToken);
        Task<bool> RemoveNotification(Guid notificationId, CancellationToken cancellationToken);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId, CancellationToken cancellationToken);
    }
}
