using System;
using System.Threading.Tasks;

namespace TaskBase.Data.NotificationService
{
    public interface INotificationService
    {
        Task Notify(Guid userId, bool isSuccess, string message);
    }
}