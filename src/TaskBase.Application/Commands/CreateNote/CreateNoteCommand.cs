using MediatR;
using Rolfin.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Core.TaskAggregate;

namespace TaskBase.Application.Commands.CreateNote
{
    public record CreateNoteCommand(Guid TaskId, string Content, DateTime CreatedAt) 
        : IRequest<Result<Note>>;
}
