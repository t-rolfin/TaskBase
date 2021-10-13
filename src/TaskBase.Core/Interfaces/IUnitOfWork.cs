using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskBase.Core.Interfaces
{
    public interface IUnitOfWork
    {
        ITaskAsyncRepository Tasks { get; }
        INotificationRepository Notifications { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
