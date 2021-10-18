using MediatR;
using Rolfin.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Application.Commands.EditNote
{
    public record EditNoteCommand(Guid TaskId, Guid NoteId, string Content)
        : IRequest<Result<bool>>;
}
