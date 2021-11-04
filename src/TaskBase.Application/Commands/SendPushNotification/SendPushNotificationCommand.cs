using MediatR;

namespace TaskBase.Application.Commands.SendPushNotification
{
    public record SendPushNotificationCommand(string Title, string Description, string UserId, bool isSuccess) 
        : IRequest<Unit>;
}
