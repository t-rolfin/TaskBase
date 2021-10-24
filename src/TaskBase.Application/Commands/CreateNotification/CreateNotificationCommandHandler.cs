using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Rolfin.Result;
using TaskBase.Application.Models;
using TaskBase.Application.Services;
using TaskBase.Core.Interfaces;
using TaskBase.Core.NotificationAggregate;

namespace TaskBase.Application.Commands.CreateNotification
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Result<NotificationModel>>
    {
        readonly INotificationService _notificationService;
        readonly IIdentityService _identityService;
        readonly ILogger<CreateNotificationCommandHandler> _logger;
        readonly IUnitOfWork _unitOfWork;

        public CreateNotificationCommandHandler(INotificationService notificationService,
            IIdentityService identityService,
            ILogger<CreateNotificationCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _identityService = identityService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<NotificationModel>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var assignedUser = await _unitOfWork.Tasks.GetUserByIdAsync(request.UserId);

            var notification = new Notification(request.Title, request.Description, request.UserId);
            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var notificationModel = new NotificationModel(notification.Id, notification.Title, notification.Description);

            return Result<NotificationModel>.Success(notificationModel)
                .With($"The task was assign to {assignedUser.FullName}.");
        }
    }
}
