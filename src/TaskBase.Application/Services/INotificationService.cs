using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskBase.Application.Services
{
    public interface INotificationService : IDisposable
    {
        Task Notify(Guid userId, bool isSuccess, string message, CancellationToken cancellationToken);
        Task PersistentNotification(Guid id, Guid userId, bool isSuccess, string description, string title, CancellationToken cancellationToken);
    }
}