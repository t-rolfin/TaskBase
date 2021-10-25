using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Rolfin.Result;
using TaskBase.Application.Services;
using TaskBase.Core.Interfaces;

namespace TaskBase.Application.Commands.RemoveNotification
{
    public class RemoveNotificationCommandHandler : IRequestHandler<RemoveNotificationCommand, Result<bool>>
    {
        readonly INotificationService _notificationService;
        readonly IIdentityService _identityService;
        readonly ILogger<RemoveNotificationCommandHandler> _logger;
        readonly IUnitOfWork _unitOfWork;

        public RemoveNotificationCommandHandler(INotificationService notificationService,
            IIdentityService identityService,
            ILogger<RemoveNotificationCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _identityService = identityService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(RemoveNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await _unitOfWork.Notifications.GetNotificationAsync(request.NotificationId, cancellationToken);
            await _unitOfWork.Notifications.Remove(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"---> The notification: {request.NotificationId} was deleted by: {notification.UserId}.");

            return Result<bool>.Success().With("The notification was deleted.");
        }
    }
}
