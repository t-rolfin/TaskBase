using OneOf;
using System.Net.Http;
using System.Threading.Tasks;
using TaskBase.Components.Models;

namespace TaskBase.Components.Utils
{
    public interface INotificationSender
    {
        NotificationSender.PageNotificationMethods GetResponse<T>(HttpResponseMessage httpResponse, out T result);
        NotificationSender.PageNotificationMethods GetResponse<T>(HttpResponseMessage httpResponse, out OneOf<T, FailApiResponse> result);
        Task<NotificationSender.PageNotificationMethods> GetResponseAsync(HttpResponseMessage httpResponse);
    }
}