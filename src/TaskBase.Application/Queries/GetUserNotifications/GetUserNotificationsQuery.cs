using System;
using System.Collections.Generic;
using MediatR;
using Rolfin.Result;
using TaskBase.Application.Models;

namespace TaskBase.Application.Queries.GetUserNotifications
{
    public record GetUserNotificationsQuery(Guid UserId) : IRequest<Result<IEnumerable<NotificationModel>>>;
}
