using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskBase.Core.Interfaces
{
    public interface IAsyncRepository<T, I>
        where T : IRootAggregate
    {
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task Remove(T entity, CancellationToken cancellationToken = default);
    }
}
