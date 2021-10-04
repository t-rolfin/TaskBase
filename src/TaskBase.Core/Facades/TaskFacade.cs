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

namespace TaskBase.Core.Facades
{
    public class TaskFacade : ITaskFacade
    {
        private readonly ITaskAsyncRepository _taskRepository;

        public TaskFacade(ITaskAsyncRepository taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException("The TaskRepository from TaskFacade is null!");
        }

        public async Task<bool> ChangeTaskState(Guid taskId, TaskState taskState, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _taskRepository.GetTaskAsync(taskId);
                if (task == default)
                    return false;

                task.ChangeTaskState(taskState);
                await _taskRepository.UpdateAsync(task, cancellationToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<TaskAggregate.Task> CreateTaskAsync(string title, string description, DateTime dueDate, Guid userId, string userName, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _taskRepository.GetUserByIdAsync(userId);

                if(user is null)
                {
                    user = new User(userId, userName);
                }

                var task = new TaskAggregate.Task(title, description, dueDate, user);
                await _taskRepository.AddAsync(task, cancellationToken);

                return task;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteTaskAsync(Guid taskId, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _taskRepository.GetTaskAsync(taskId);
                await _taskRepository.RemoveTask(task);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<TaskAggregate.Task> GetTaskDetailsAsync(Guid taskId)
        {
            return await _taskRepository.GetTaskAsync(taskId);
        }

        public async Task<IEnumerable<TaskAggregate.Task>> GetTasksByUserIdAsync(Guid userId)
        {
            return await _taskRepository.GetTasksByUserAsync(userId);
        }

        public async Task<User> GetUserByNameAsync(string userName)
        {
            return await _taskRepository.GetUserByUserNameAsync(userName);
        }

        public async Task<bool> EditDescriptionAsync(string taskId, string newDescription, CancellationToken cancellationToken)
        {
            try
            {
                var taskDetails = await GetTaskDetailsAsync(Guid.Parse(taskId));
                taskDetails.EditDescription(newDescription);

                await _taskRepository.UpdateAsync(taskDetails, cancellationToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EditTitleAsync(string taskId, string newTitle, CancellationToken cancellationToken)
        {
            try
            {
                var taskDetails = await GetTaskDetailsAsync(Guid.Parse(taskId));
                taskDetails.EditTitle(newTitle);

                await _taskRepository.UpdateAsync(taskDetails, cancellationToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Note> CreateNoteAsync(string taskId, string noteContent, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _taskRepository.GetTaskAsync(Guid.Parse(taskId));
                var note = task.CreateNote(noteContent);

                await _taskRepository.UpdateAsync(task, cancellationToken);

                return note;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> EditNoteAsync(string taskId, string noteId, string newContent, CancellationToken cancellationToken)
        {
            try
            {
                var task = await GetTaskDetailsAsync(Guid.Parse(taskId));
                task.EditeNote(Guid.Parse(noteId), newContent);

                await _taskRepository.UpdateAsync(task, cancellationToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Note>> GetTaskNotesAsync(string taskId, CancellationToken cancellationToken)
        {
            try
            {
                return await _taskRepository.GetTaskNotesAsync(Guid.Parse(taskId), cancellationToken);
            }
            catch (Exception)
            {
                return new List<Note>();
            }
        }
    }
}
