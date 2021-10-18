using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Application.Commands.SetPriorityLevel
{
    public class SetPriorityLevelValidator : AbstractValidator<SetPriorityLevelCommand>
    {
        public SetPriorityLevelValidator()
        {
            RuleFor(x => x.TaskId).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(x => x.PriorityLevelKey).LessThan(1).GreaterThan(3);
        }
    }
}
