using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rolfin.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Commands.CreateNotification;
using TaskBase.Application.Commands.RemoveNotification;
using TaskBase.Application.Commands.SendPageNotification;
using TaskBase.Application.Commands.SendPushNotification;
using TaskBase.Application.Models;
using TaskBase.Application.Queries.GetUserNotifications;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(Roles = "Member, Admin")]
    public class NotificationController : BaseController
    {
        /// <summary>
        /// Returns user notifications.
        /// </summary>
        [HttpGet]
        [Route("notifications/{userId:Guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<NotificationModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetNotifications([FromRoute]Guid userId, CancellationToken cancellationToken)
        {
            GetUserNotificationsQuery query = new(userId);
            return await SendWithMediator(query, cancellationToken);
        }

        /// <summary>
        /// Create a notification.
        /// </summary>
        [HttpPost]
        [Route("notification")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(NotificationModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateNotification(CreateNotificationCommand model, CancellationToken cancellationToken)
        {
            return await SendCreateWithMediator(model, cancellationToken);
        }

        /// <summary>
        /// Send a push notification.
        /// </summary>
        [HttpPost]
        [Route("notification/push")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PushNotification(SendPushNotificationCommand model, CancellationToken cancellationToken)
        {
            await Mediator.Send(model, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Send a page notification.
        /// </summary>
        [HttpPost]
        [Route("notification/page")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PageNotification(SendPageNotificationCommand model, CancellationToken cancellationToken)
        {
            await Mediator.Send(model, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Remove user notification.
        /// </summary>
        [HttpDelete]
        [Route("notification/{notificationId:Guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveNotification([FromRoute] RemoveNotificationCommand model, CancellationToken cancellationToken)
        {
            return await SendWithMediator(model, cancellationToken: cancellationToken);
        }
    }
}
