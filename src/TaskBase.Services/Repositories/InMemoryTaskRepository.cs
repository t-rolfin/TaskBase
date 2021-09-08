using Microsoft.EntityFrameworkCore;
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
        private readonly TaskDbContext _context;

        public InMemoryTaskRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<CoreTask> AddAsync(CoreTask entity, CancellationToken cancellationToken = default)
        {
            await _context.Tasks.AddAsync(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<CoreTask> UpdateAsync(CoreTask entity, CancellationToken cancellationToken = default)
        {
            var oldTask = _context.Tasks.FirstOrDefault(x => x.Id == entity.Id);

            if (oldTask == default)
                throw new TaskNotFoundException("The specified task wasn't found!");
            else
            {
                _context.Tasks.Update(oldTask);
                await _context.SaveChangesAsync(cancellationToken);
                return await GetTaskAsync(entity.Id);
            }
        }

        public async Task RemoveTask(CoreTask task, CancellationToken cancellationToken = default)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<CoreTask> GetTaskAsync(Guid taskId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

            if (task == default)
                throw new TaskNotFoundException("The specified task doesn't exist!");
            else
                return task;
        }

        public async Task<IEnumerable<CoreTask>> GetTasksAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                return _context.Tasks.ToList();
            });
        }
    }
}
