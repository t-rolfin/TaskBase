using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Application.Commands.ChangeTaskState
{
    public class ChangeTaskStateValidator : AbstractValidator<ChangeTaskStateCommand>
    {
        public ChangeTaskStateValidator()
        {
            RuleFor(x => x.TaskId).NotEqual(Guid.Empty);
        }
    }
}
