using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rolfin.Result;
using TaskBase.Application.Models;
using TaskBase.Core.Interfaces;
using TaskBase.Core.NotificationAggregate;

namespace TaskBase.Application.Commands.CreateNotification
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Result<NotificationModel>>
    {
        readonly IUnitOfWork _unitOfWork;

        public CreateNotificationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<NotificationModel>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var assignedUser = await _unitOfWork.Tasks.GetUserByIdAsync(request.UserId);

                var notification = new Notification(request.Title, request.Description, request.UserId);
                await _unitOfWork.Notifications.AddAsync(notification);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var notificationModel = new NotificationModel(notification.Id, notification.Title, notification.Description);

                return Result<NotificationModel>.Success(notificationModel)
                    .With($"The task was assign to {assignedUser.FullName}.");
            }
            catch (Exception ex)
            {
                return Result<NotificationModel>.Invalid().With(ex.Message);
            }
        }
    }
}
