using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Core.Interfaces;
using TaskBase.Core.TaskAggregate;

namespace TaskBase.Core.Facades
{
    public class TaskFacade : ITaskFacade
    {
        private readonly ITaskAsyncRepository _taskRepository;

        public TaskFacade(ITaskAsyncRepository taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException("The TaskRepository from TaskFacade is null!");
        }

        public async Task<bool> CloseTaskAsync(Guid taskId, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _taskRepository.GetTaskAsync(taskId);
                if (task == default)
                    return false;

                task.CompleteTask();
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
                var user = new User(userId, userName);
                var task = new TaskAggregate.Task(title, description, dueDate, user);
                await _taskRepository.AddAsync(task, cancellationToken);

                return task;
            }
            catch
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

        public async Task<bool> SetTaskAsInProgressAsync(Guid taskId, CancellationToken cancellationToken)
        {
            try
            {
                var task = await GetTaskDetailsAsync(taskId);
                task.StartWorking();
                await _taskRepository.UpdateAsync(task);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
