using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rolfin.Result;
using TaskBase.Core.Interfaces;
using TaskBase.Core.NotificationAggregate;

namespace TaskBase.Application.Commands.CreateNotification
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Result<Notification>>
    {
        readonly IUnitOfWork _unitOfWork;

        public CreateNotificationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Notification>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var assignedUser = await _unitOfWork.Tasks.GetUserByIdAsync(request.UserId);

                var notification = new Notification(request.Title, request.Description, request.UserId);
                await _unitOfWork.Notifications.AddAsync(notification);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<Notification>.Success(notification).With($"The task was assign to {assignedUser.FullName}.");
            }
            catch (Exception ex)
            {
                return Result<Notification>.Invalid().With(ex.Message);
            }
        }
    }
}
