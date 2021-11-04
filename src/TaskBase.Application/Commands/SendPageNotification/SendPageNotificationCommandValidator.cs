using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Application.Commands.SendPageNotification
{
    public class SendPageNotificationCommandValidator 
        : AbstractValidator<SendPageNotificationCommand>
    {
        public SendPageNotificationCommandValidator()
        {
            RuleFor(x => x.UserId)
                .Must(x => x != Guid.Empty.ToString())
                .Must(x => Guid.TryParse(x, out Guid id));

            RuleFor(x => x.Message)
                .NotEmpty()
                .NotNull();
        }
    }
}
