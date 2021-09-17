using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Core.TaskAggregate;

namespace TaskBase.Core.Interfaces
{
    public interface IAsyncRepository<T, I>
        where T : IRootAggregate
    {
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        System.Threading.Tasks.Task RemoveTask(T task, CancellationToken cancellationToken = default);
        Task<T> GetTaskAsync(I taskId);
        Task<IEnumerable<T>> GetTasksByUserAsync(Guid userId);
        Task<User> GetUserById(Guid userId);
    }
}
