﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Application.Commands.UpdateTask
{
    public class UpdateTaskValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskValidator()
        {
            RuleFor(x => x.TaskId).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
        }
    }
}
