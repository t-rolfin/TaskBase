using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Models;
using TaskBase.Application.Services;
using TaskBase.Components.Models;
using TaskBase.Components.Services;
using TaskBase.Core.Interfaces;

namespace TaskBase.Components.Views.Shared.Components.Notifications
{
    [ViewComponent( Name = "Notifications")]
    public class NotificationsViewComponent : ViewComponent
    {
        readonly IQueryRepository _queryReopsitory;
        readonly IIdentityService _identityProvider;

        public NotificationsViewComponent(IQueryRepository queryReopsitory, IIdentityService identityProvider)
        {
            _queryReopsitory = queryReopsitory;
            _identityProvider = identityProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUserId = _identityProvider.GetCurrentUserIdentity();
            var userNotifications = await _queryReopsitory.GetUserNotificationsAsync(currentUserId, default(CancellationToken));
            var notifications = userNotifications.Select(x =>
            {
                return new NotificationModel(x.Id, x.Title, x.Description);
            });
            var model = new NotificationsModel(notifications.ToList(), notifications.Count());

            return View(model);
        }
    }
}
