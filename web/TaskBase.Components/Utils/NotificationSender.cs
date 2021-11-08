using Microsoft.AspNetCore.Http;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Components.Services;

namespace TaskBase.Components.Utils
{
    public class NotificationSender : INotificationSender
    {
        private readonly PageNotificationMethods _methods;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _contextAccessor;

        private object _result { get; set; }

        public NotificationSender(
            INotificationService notificationService,
            IHttpContextAccessor contextAccessor)
        {
            _notificationService = notificationService;
            _contextAccessor = contextAccessor;
            _methods = new PageNotificationMethods(_notificationService, _contextAccessor, this);
        }

        public PageNotificationMethods GetResponse<T>(HttpResponseMessage httpResponse, out T result)
        {
            if (httpResponse.IsSuccessStatusCode)
            {
                result = httpResponse.MapTo<T>().Result;
                _result = httpResponse.MapTo<T>().Result;
            }
            else
            {
                result = default(T);
                _result = httpResponse.MapTo<FailApiResponse>().Result;
            }

            return this._methods;
        }

        public PageNotificationMethods GetResponse<T>(HttpResponseMessage httpResponse, out OneOf<T, FailApiResponse> result)
        {
            if (httpResponse.IsSuccessStatusCode)
            {
                result = httpResponse.MapTo<T>().Result;
                _result = httpResponse.MapTo<T>().Result;
            }
            else
            {
                result = httpResponse.MapTo<FailApiResponse>().Result;
                _result = httpResponse.MapTo<FailApiResponse>().Result;
            }

            return this._methods;
        }

        public async Task<PageNotificationMethods> GetResponseAsync(HttpResponseMessage httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode)
                _result = new object();
            else
                _result = await httpResponse.MapTo<FailApiResponse>();

            return this._methods;
        }


        public class PageNotificationMethods
        {
            private readonly INotificationService _notificationService;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly NotificationSender _base;

            private PageNotificationModel PageNotificationModel = null;
            private PushNotificationModel PushNotificationModel = null;

            public PageNotificationMethods(
                INotificationService notificationService,
                IHttpContextAccessor contextAccessor,
                NotificationSender baseClass)
            {
                _notificationService = notificationService;
                _contextAccessor = contextAccessor;
                _base = baseClass;
            }

            public PageNotificationMethods CreatePageNotification(string message)
            {
                var userId = _contextAccessor.HttpContext
                    .User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)
                    .Value;

                _ = _base._result is FailApiResponse fail
                    ? PageNotificationModel = new PageNotificationModel(userId, fail.Errors.First(), false)
                    : PageNotificationModel = new PageNotificationModel(userId, message, true);

                return this;
            }

            public PageNotificationMethods CreatePushNotification(string title, string Description, string userId)
            {
                if (string.IsNullOrWhiteSpace(userId))
                    PushNotificationModel = null;

                PushNotificationModel = new(title, Description, userId, true);

                return this;
            }

            public PageNotificationMethods CreatePushAndPageNotification(string pageNotificationMessage ,string title, string Description, string userId)
            {
                if (string.IsNullOrWhiteSpace(userId))
                    PushNotificationModel = null;
                else
                    PushNotificationModel = new(title, Description, userId, true);

                CreatePageNotification(pageNotificationMessage);

                return this;
            }

            public async Task SendAsync()
            {
                if(PageNotificationModel is not null) await _notificationService.PageNotification(PageNotificationModel);
                if(PushNotificationModel is not null) await _notificationService.PushNotification(PushNotificationModel);
            }
        }

    }
}
