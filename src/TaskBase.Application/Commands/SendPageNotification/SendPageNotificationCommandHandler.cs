using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Services;

namespace TaskBase.Application.Commands.SendPageNotification
{
    public class SendPageNotificationCommandHandler :
        IRequestHandler<SendPageNotificationCommand, Unit>
    {
        private readonly INotificationService _notificationService;

        public SendPageNotificationCommandHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<Unit> Handle(SendPageNotificationCommand request, CancellationToken cancellationToken)
        {
            await _notificationService.Notify(
                    Guid.Parse(request.UserId),
                    request.IsSuccess,
                    request.Message,
                    cancellationToken
                );

            return Unit.Value;
        }
    }
}
