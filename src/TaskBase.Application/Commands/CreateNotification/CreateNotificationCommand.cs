﻿using System;
using MediatR;
using Rolfin.Result;
using TaskBase.Core.NotificationAggregate;

namespace TaskBase.Application.Commands.CreateNotification
{
    public record CreateNotificationCommand(Guid UserId, string Title, string Description) 
        : IRequest<Result<Notification>>;
}
