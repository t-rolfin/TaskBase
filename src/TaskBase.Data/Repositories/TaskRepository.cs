using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Core.Enums;
using TaskBase.Core.Interfaces;
using TaskBase.Data.Exceptions;
using CoreTask = TaskBase.Core.TaskAggregate.Task;
using CoreUser = TaskBase.Core.TaskAggregate.User;
using CoreNote = TaskBase.Core.TaskAggregate.Note;

namespace TaskBase.Data.Repositories
{
    public class TaskRepository : ITaskAsyncRepository
    {
        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context)
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
                foreach (var note in entity.Notes)
                    _context.Entry(note).State = EntityState.Added;

                _context.Tasks.Update(entity);
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
            var task = await _context.Tasks.Where(x => x.Id == taskId).FirstOrDefaultAsync();

            if (task == default)
                throw new TaskNotFoundException("The specified task doesn't exist!");
            else
                return task;
        }

        public async Task<IEnumerable<CoreTask>> GetTasksByUserAsync(Guid userId)
        {
            return await Task.Factory.StartNew(() =>
            {
                return _context.Tasks.Where(x => x.AssignTo.Id == userId).ToList();
            });
        }

        public async Task<Core.TaskAggregate.User> GetUserByIdAsync(Guid userId)
        {
            return await _context.FindAsync<CoreUser>(userId);
        }

        public async Task<Core.TaskAggregate.User> GetUserByUserNameAsync(string userName)
        {
            return await _context.Set<CoreUser>().Where(x => x.FullName == userName).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CoreNote>> GetTaskNotesAsync(Guid taskId, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.Where(x => x.Id == taskId)
                .Include(x => x.Notes).FirstOrDefaultAsync(cancellationToken);

            return task.Notes;
        }
    }
}
