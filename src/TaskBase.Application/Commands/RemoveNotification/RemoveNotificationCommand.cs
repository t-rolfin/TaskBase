using System;
using MediatR;
using Rolfin.Result;

namespace TaskBase.Application.Commands.RemoveNotification
{
    public record RemoveNotificationCommand(Guid NotificationId) : IRequest<Result<bool>>;
}
