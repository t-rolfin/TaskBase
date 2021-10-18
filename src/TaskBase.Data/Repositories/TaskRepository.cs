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
using CorePriorityLevel = TaskBase.Core.TaskAggregate.PriorityLevel;

namespace TaskBase.Data.Repositories
{
    public class TaskRepository : BaseRepository<CoreTask>, ITaskAsyncRepository
    {
        public TaskRepository(TaskDbContext context) : base(context) { }

        public async Task<CoreTask> AddAsync(CoreTask entity, CancellationToken cancellationToken = default)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<CoreTask> UpdateAsync(CoreTask entity, CancellationToken cancellationToken = default)
        {
            var oldTask = dbSet.Where(x => x.Id == entity.Id)
                .Include(x => x.Notes)
                .ToList();

            if (oldTask == default)
                throw new TaskNotFoundException("The specified task wasn't found!");
            else
            {
                CheckForModifiedNotes(entity);
                dbSet.Update(entity);
    
                return await GetTaskAsync(entity.Id);
            }
        }

        public async Task Remove(CoreTask task, CancellationToken cancellationToken = default)
        {
            dbSet.Remove(task);
            await Task.CompletedTask;
        }

        public async Task<CoreTask> GetTaskAsync(Guid taskId)
        {
            var task = await dbSet.Where(x => x.Id == taskId).Include(x => x.PriorityLevel)
                .FirstOrDefaultAsync();

            if (task == default)
                throw new TaskNotFoundException("The specified task doesn't exist!");
            else
                return task;
        }

        public async Task<IEnumerable<CoreTask>> GetTasksByUserAsync(Guid userId)
        {
            return await Task.Factory.StartNew(() =>
            {
                return dbSet.Where(x => x.AssignTo.Id == userId)
                .Include(x => x.PriorityLevel).ToList();
            });
        }

        public async Task<CoreUser> GetUserByIdAsync(Guid userId)
        {
            var user = await _context.FindAsync<CoreUser>(userId);
            
            if (user is null)
                throw new UserNotFoundException("Searched user wasn't found.");

            return user;
        }

        public async Task<CoreUser> GetUserByUserNameAsync(string userName)
        {
            var user = await _context.Set<CoreUser>().Where(x => x.FullName == userName).FirstOrDefaultAsync();
            
            if (user == default)
                throw new UserNotFoundException("Searched user wasn't found.");

            return user;
        }

        public async Task<IEnumerable<CoreNote>> GetTaskNotesAsync(Guid taskId, CancellationToken cancellationToken)
        {
            var task = await dbSet.Where(x => x.Id == taskId)
                .Include(x => x.Notes).FirstOrDefaultAsync(cancellationToken);

            return task.Notes;
        }

        public async Task<CoreTask> GetTaskWithNotesAsync(Guid taskId)
        {
            var task = await dbSet.Where(x => x.Id == taskId)
                .Include(x => x.Notes).FirstOrDefaultAsync();

            if (task == default)
                throw new TaskNotFoundException("The specified task doesn't exist!");
            else
                return task;
        }

        public async Task<CorePriorityLevel> GetPriorityLevelAsync(int value)
        {
            var priorityLevel = await _context.Set<CorePriorityLevel>().FirstOrDefaultAsync(x => x.Value == value);

            if (priorityLevel == default)
                throw new PriorityLevelNotFoundException("The specified priority level wasn't found.");

            return priorityLevel;
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
