using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rolfin.Result;
using TaskBase.Core.Interfaces;

namespace TaskBase.Application.Commands.RemoveNotification
{
    public class RemoveNotificationCommandHandler : IRequestHandler<RemoveNotificationCommand, Result<bool>>
    {
        readonly IUnitOfWork _unitOfWork;

        public RemoveNotificationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(RemoveNotificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var notification = await _unitOfWork.Notifications.GetNotificationAsync(request.NotificationId, cancellationToken);
                await _unitOfWork.Notifications.Remove(notification);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success().With("The notification was deleted.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Invalid().With(ex.Message);
            }
        }
    }
}
