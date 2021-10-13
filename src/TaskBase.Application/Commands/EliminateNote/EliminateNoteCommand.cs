using MediatR;
using Rolfin.Result;
using System;

namespace TaskBase.Application.Commands.EliminateNote
{
    public record EliminateNoteCommand(Guid TaskId, Guid NoteId) : IRequest<Result<bool>>;
}
