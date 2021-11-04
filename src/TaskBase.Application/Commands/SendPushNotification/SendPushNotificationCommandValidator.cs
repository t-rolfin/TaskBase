using FluentValidation;
using System;

namespace TaskBase.Application.Commands.SendPushNotification
{
    public class SendPushNotificationCommandValidator
        : AbstractValidator<SendPushNotificationCommand>
    {
        public SendPushNotificationCommandValidator()
        {
            RuleFor(x => x.UserId)
                .Must(x => x != Guid.Empty.ToString())
                .Must(x => Guid.TryParse(x, out Guid id));

            RuleFor(x => x.Title)
                .NotNull().NotEmpty(); 
            
            RuleFor(x => x.Description)
                .NotNull().NotEmpty();
        }
    }
}
