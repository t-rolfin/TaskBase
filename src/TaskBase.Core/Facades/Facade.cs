using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Core.Enums;
using TaskBase.Core.Interfaces;
using TaskBase.Core.TaskAggregate;
using BaseTask = TaskBase.Core.TaskAggregate.Task;
using Microsoft.AspNetCore.SignalR.Client;
using TaskBase.Core.NotificationAggregate;
using Rolfin.Result;

namespace TaskBase.Core.Facades
{
    public class Facade : IFacade
    {
        private HubConnection _hubConnection;
        private readonly IUnitOfWork _unitOfWork;

        public Facade(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException("The TaskRepository from TaskFacade is null!");
        }

        public async Task<Result<bool>> ChangeTaskState(Guid taskId, TaskState taskState, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetTaskAsync(taskId);
                if (task == default)
                    return false;

                task.ChangeTaskState(taskState);
                await _unitOfWork.Tasks.UpdateAsync(task, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success(true).With($"The state of task was successfully change in to ${nameof(taskState)}");
            }
            catch (Exception ex)
            {
                return Result<bool>.Invalid().With(ex.Message);
            }
        }

        public async Task<Result<BaseTask>> CreateTaskAsync(string title, string description, DateTime dueDate, Guid userId, string userName, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _unitOfWork.Tasks.GetUserByIdAsync(userId);

                if(user is null)
                {
                    user = new User(userId, userName);
                }

                var task = new BaseTask(title, description, dueDate, user);
                await _unitOfWork.Tasks.AddAsync(task, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await Nofity(userId, title, "A new task was assign to you, refresh 'To Do' container to see the task.", cancellationToken);

                return Result.Success(task).With("A new task was successfully created.");
            }
            catch (Exception ex)
            {
                return Result<BaseTask>.Invalid().With(ex.Message);
            }
            finally
            {
                await _hubConnection.StopAsync();
            }
        }

        public async Task<Result<bool>> DeleteTaskAsync(Guid taskId, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetTaskAsync(taskId);
                await _unitOfWork.Tasks.Remove(task);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success().With("The task was removed!");
            }
            catch(Exception ex)
            {
                return Result<bool>.Invalid().With(ex.Message);
            }
        }

        public async Task<TaskAggregate.Task> GetTaskDetailsAsync(Guid taskId)
        {
            return await _unitOfWork.Tasks.GetTaskAsync(taskId);
        }

        public async Task<IEnumerable<TaskAggregate.Task>> GetTasksByUserIdAsync(Guid userId)
        {
            return await _unitOfWork.Tasks.GetTasksByUserAsync(userId);
        }

        public async Task<User> GetUserByNameAsync(string userName)
        {
            return await _unitOfWork.Tasks.GetUserByUserNameAsync(userName);
        }

        public async Task<Result<bool>> EditDescriptionAsync(string taskId, string newDescription, CancellationToken cancellationToken)
        {
            try
            {
                var taskDetails = await GetTaskDetailsAsync(Guid.Parse(taskId));
                taskDetails.EditDescription(newDescription);

                await _unitOfWork.Tasks.UpdateAsync(taskDetails, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success().With("Description for the task was changed!");
            }
            catch (Exception ex)
            {
                return Result<bool>.Invalid().With(ex.Message);
            }
        }

        public async Task<Result<bool>> EditTitleAsync(string taskId, string newTitle, CancellationToken cancellationToken)
        {
            try
            {
                var taskDetails = await GetTaskDetailsAsync(Guid.Parse(taskId));
                taskDetails.EditTitle(newTitle);

                await _unitOfWork.Tasks.UpdateAsync(taskDetails, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success().With("Title was updated!");
            }
            catch (Exception ex)
            {
                return Result<bool>.Invalid().With(ex.Message);
            }
        }

        public async Task<Result<Note>> CreateNoteAsync(string taskId, string noteContent, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetTaskAsync(Guid.Parse(taskId));
                var note = task.CreateNote(noteContent);

                await _unitOfWork.Tasks.UpdateAsync(task, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<Note>.Success(note)
                    .With($"A note was added to the task { task.Title.Take(10).ToString().PadRight(3, '.') }");
            }
            catch (Exception ex)
            {
                return Result<Note>.Invalid().With(ex.Message);
            }
        }

        public async Task<Result<bool>> EditNoteAsync(string taskId, string noteId, string newContent, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetTaskWithNotesAsync(Guid.Parse(taskId));
                task.EditeNote(Guid.Parse(noteId), newContent);

                await _unitOfWork.Tasks.UpdateAsync(task, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success().With($"The note was successfully updated.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Invalid().With(ex.Message);
            }
        }

        public async Task<IEnumerable<Note>> GetTaskNotesAsync(string taskId, CancellationToken cancellationToken)
        {
            try
            {
                return await _unitOfWork.Tasks.GetTaskNotesAsync(Guid.Parse(taskId), cancellationToken);
            }
            catch
            {
                return new List<Note>();
            }
        }

        public async Task<Result<bool>> EliminateNoteFromTaskAsync(string taskId, string noteId, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetTaskWithNotesAsync(Guid.Parse(taskId));
                task.EliminateNote(Guid.Parse(noteId));
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _unitOfWork.Tasks.UpdateAsync(task, cancellationToken);

                return Result<bool>.Success().With($"The specified note was deleted!");
            }
            catch (Exception ex)
            {
                return Result<bool>.Invalid().With(ex.Message);
            }
        }



        public async Task<Result<Notification>> CreateNotificationAsync(Guid userId, string title, string description, CancellationToken cancellationToken)
        {
            try
            {
                var notification = new Notification(title, description, userId);
                await _unitOfWork.Notifications.AddAsync(notification);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var assignedUser = await _unitOfWork.Tasks.GetUserByIdAsync(userId);

                return Result<Notification>.Success(notification).With($"The task was assign to {assignedUser.FullName}.");
            }
            catch (Exception ex)
            {
                return Result<Notification>.Invalid().With(ex.Message);
            }
        }

        public async Task<Result<bool>> RemoveNotification(Guid notificationId, CancellationToken cancellationToken)
        {
            try
            {
                var notification = await _unitOfWork.Notifications.GetNotificationAsync(notificationId, cancellationToken);
                await _unitOfWork.Notifications.Remove(notification);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success().With("The notification was deleted.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Invalid().With(ex.Message);
            }
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Notifications.GetUserNotificationsAsync(userId, cancellationToken);
        }

        /// <summary>
        /// Create a connection with SignalR Hub and send a notification to it.
        /// </summary>
        /// <param name="userId">Is the ID of user that will be nitified.</param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async System.Threading.Tasks.Task Nofity(Guid userId, string title, string description, CancellationToken cancellationToken)
        {
            try
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl(new Uri("https://localhost:5004/notificationhub"), x =>
                    {
                        x.AccessTokenProvider = () => System.Threading.Tasks.Task.FromResult(userId.ToString());
                    }).Build();

                await _hubConnection.StartAsync();

                var notification = await CreateNotificationAsync(userId, title, description, cancellationToken);

                await _hubConnection.InvokeAsync("SendMessage", notification.Value);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
