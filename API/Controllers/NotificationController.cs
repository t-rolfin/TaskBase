using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Commands.CreateNotification;
using TaskBase.Application.Commands.RemoveNotification;
using TaskBase.Application.Queries.GetUserNotifications;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(Roles = "Member, Admin")]
    public class NotificationController : BaseController
    {
        [HttpGet]
        [Route("notifications")]
        public async Task<IActionResult> GetNotifications(GetUserNotificationsQuery model, CancellationToken cancellationToken)
        {
            return await SendWithMediator(model, true, cancellationToken);
        }

        [HttpPost]
        [Route("notification/create")]
        public async Task<IActionResult> CreateNotification(CreateNotificationCommand model, CancellationToken cancellationToken)
        {
            return await SendCreateWithMediator(model, cancellationToken);
        }

        [HttpPost]
        [Route("notification/delete")]
        public async Task<IActionResult> RemoveNotification(RemoveNotificationCommand model, CancellationToken cancellationToken)
        {
            return await SendWithMediator(model, cancellationToken: cancellationToken);
        }
    }
}
