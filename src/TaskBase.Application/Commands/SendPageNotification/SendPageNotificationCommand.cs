using MediatR;

namespace TaskBase.Application.Commands.SendPageNotification
{
    public record SendPageNotificationCommand(string UserId, string Message, bool IsSuccess) : IRequest<Unit>;
}
