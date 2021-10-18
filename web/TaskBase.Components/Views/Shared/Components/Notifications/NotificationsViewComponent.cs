using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Models;
using TaskBase.Application.Queries.GetUserNotifications;
using TaskBase.Application.Services;
using TaskBase.Components.Models;
using TaskBase.Components.Services;
using TaskBase.Core.Interfaces;

namespace TaskBase.Components.Views.Shared.Components.Notifications
{
    [ViewComponent( Name = "Notifications")]
    public class NotificationsViewComponent : ViewComponent
    {
        readonly IMediator _mediator;
        readonly IIdentityService _identityProvider;

        public NotificationsViewComponent(IMediator mediator, IIdentityService identityProvider)
        {
            _mediator = mediator;
            _identityProvider = identityProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUserId = _identityProvider.GetCurrentUserIdentity();

            GetUserNotificationsQuery command = new(currentUserId);
            var result = await _mediator.Send(command);

            var model = new NotificationsModel(
                    result.IsSuccess ? result.Value.ToList() : new List<NotificationModel>(),
                    result.IsSuccess ? result.Value.Count() : 0
                );

            return View(model);
        }
    }
}
