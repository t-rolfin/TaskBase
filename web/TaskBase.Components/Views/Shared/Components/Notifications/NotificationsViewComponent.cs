using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Services;
using TaskBase.Components.Models;
using TaskBase.Components.Services;
using TaskBase.Core.Interfaces;

namespace TaskBase.Components.Views.Shared.Components.Notifications
{
    [ViewComponent( Name = "Notifications")]
    public class NotificationsViewComponent : ViewComponent
    {
        readonly IFacade _facade;
        readonly IIdentityService _identityProvider;

        public NotificationsViewComponent(IFacade facade, IIdentityService identityProvider)
        {
            _facade = facade;
            _identityProvider = identityProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUserId = _identityProvider.GetCurrentUserIdentity();
            var userNotifications = await _facade.GetUserNotificationsAsync(currentUserId, default(CancellationToken));
            var notifications = userNotifications.Select(x =>
            {
                return new NotificationModel(x.Id, x.Title, x.Description);
            });
            var model = new NotificationsModel(notifications.ToList(), notifications.Count());

            return View(model);
        }
    }
}
