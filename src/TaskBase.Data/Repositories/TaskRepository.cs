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
            var oldTask = _context.Tasks.Where(x => x.Id == entity.Id)
                .Include(x => x.Notes)
                .ToList();

            if (oldTask == default)
                throw new TaskNotFoundException("The specified task wasn't found!");
            else
            {
                CheckForModifiedNotes(entity);
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

        public async Task<CoreUser> GetUserByIdAsync(Guid userId)
        {
            return await _context.FindAsync<CoreUser>(userId);
        }

        public async Task<CoreUser> GetUserByUserNameAsync(string userName)
        {
            return await _context.Set<CoreUser>().Where(x => x.FullName == userName).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CoreNote>> GetTaskNotesAsync(Guid taskId, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.Where(x => x.Id == taskId)
                .Include(x => x.Notes).FirstOrDefaultAsync(cancellationToken);

            return task.Notes;
        }

        public async Task<CoreTask> GetTaskWithNotesAsync(Guid taskId)
        {
            var task = await _context.Tasks.Where(x => x.Id == taskId)
                .Include(x => x.Notes).FirstOrDefaultAsync();

            if (task == default)
                throw new TaskNotFoundException("The specified task doesn't exist!");
            else
                return task;
        }

        /// <summary>
        /// Check every note from 'entity' if is modified and 
        /// set EntityState as: Modified, Added, Removed or Unchanged.
        /// </summary>
        /// <param name="entity">Note entity received from database.</param>
        private void CheckForModifiedNotes(CoreTask entity)
        {
            foreach (var note in entity.Notes)
                if (note.IsModified)
                    switch (note.EntityStatus)
                    {
                        case EntityStatus.Added:
                            _context.Entry(note).State = EntityState.Added;
                            break;
                        case EntityStatus.Modified:
                            _context.Entry(note).State = EntityState.Modified;
                            break;
                        case EntityStatus.Deleted:
                            _context.Entry(note).State = EntityState.Deleted;
                            break;
                        default:
                            _context.Entry(note).State = EntityState.Unchanged;
                            break;
                    }
        }
    }
}
