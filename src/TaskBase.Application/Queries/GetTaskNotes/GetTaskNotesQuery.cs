using System;
using System.Collections.Generic;
using MediatR;
using Rolfin.Result;
using TaskBase.Application.Models;

namespace TaskBase.Application.Queries.GetTaskNotes
{
    public record GetTaskNotesQuery(Guid TaskId) : IRequest<Result<IEnumerable<NoteModel>>>;

}
