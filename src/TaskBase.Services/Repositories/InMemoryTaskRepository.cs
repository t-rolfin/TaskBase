using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Core.Interfaces;
using TaskBase.Services.Exceptions;
using CoreTask = TaskBase.Core.TaskAggregate.Task;

namespace TaskBase.Services.Repositories
{
    public class InMemoryTaskRepository : ITaskAsyncRepository
    {

        private readonly List<CoreTask> _tasks;

        public InMemoryTaskRepository()
        {
            _tasks = new();
        }

        public async Task<CoreTask> AddAsync(CoreTask entity, CancellationToken cancellationToken)
        {
            return await Task.Factory.StartNew(() =>
            {
                _tasks.Add(entity);
                return entity;
            });
        }

        public async Task<CoreTask> GetTaskAsync(Guid taskId)
        {
            return await Task.Factory.StartNew(() =>
            {
                return _tasks.FirstOrDefault(x => x.Id == taskId);
            });
        }

        public async Task<IEnumerable<CoreTask>> GetTasksAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                return _tasks;
            });
        }

        public async Task<CoreTask> UpdateAsync(CoreTask entity, CancellationToken cancellationToken)
        {
            var newTask = await Task.Factory.StartNew( async () =>
            {
                var oldTask = _tasks.FirstOrDefault(x => x.Id == entity.Id);

                if (oldTask == default)
                    throw new TaskNotFoundException("The specified task wasn't found!");
                else
                {
                    _tasks.Remove(oldTask);
                    var newTask = await AddAsync(entity, cancellationToken);
                    return newTask;
                }
            });

            return await newTask;
        }
    }
}
