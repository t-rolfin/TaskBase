using System;
using System.Threading.Tasks;

namespace TaskBase.Application.Services
{
    public interface INotificationService
    {
        Task Notify(Guid userId, bool isSuccess, string message);
    }
}