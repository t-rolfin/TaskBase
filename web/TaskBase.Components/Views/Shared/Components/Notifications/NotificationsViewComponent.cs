using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Components.Services;

namespace TaskBase.Components.Views.Shared.Components.Notifications
{
    [ViewComponent( Name = "Notifications")]
    public class NotificationsViewComponent : ViewComponent
    {
        private readonly INotificationService _notificationService;

        public NotificationsViewComponent(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Guid.TryParse(
                    HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value,
                    out Guid currentUserId
                    );

            NotificationsModel notificationsModel = await _notificationService
                .GetNotificationsAsync(currentUserId);

            return View(notificationsModel);
        }
    }
}
