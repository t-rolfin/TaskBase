using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Application.Commands.EditNote
{
    public class EditNoteValidator : AbstractValidator<EditNoteCommand>
    {
        public EditNoteValidator()
        {
            RuleFor(x => x.TaskId).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(x => x.NoteId).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}
