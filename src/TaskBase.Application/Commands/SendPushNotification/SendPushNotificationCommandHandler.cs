using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Models;
using TaskBase.Application.Services;
using TaskBase.Core.Interfaces;
using TaskBase.Core.NotificationAggregate;

namespace TaskBase.Application.Commands.SendPushNotification
{
    public class SendPushNotificationCommandHandler : IRequestHandler<SendPushNotificationCommand, Unit>
    {
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;

        public SendPushNotificationCommandHandler(INotificationService notificationService,
                                                  IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(SendPushNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = new Notification(request.Title, request.Description, Guid.Parse(request.UserId));
            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var notificationModel = new NotificationModel(notification.Id, notification.Title, notification.Description);

            if (notification is not null)
                await _notificationService.PushNotification(
                        notificationModel.Id,
                        request.UserId.ToString(),
                        request.isSuccess,
                        notificationModel.Description,
                        notificationModel.Title,
                        cancellationToken
                    );  

            return Unit.Value;
        }
    }
}
