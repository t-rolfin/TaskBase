using MediatR;
using Rolfin.Result;
using System;
using System.Collections.Generic;
using TaskBase.Application.Models;

namespace TaskBase.Application.Queries.GetTasksByUser
{
    public record GetTasksByUserQuery(Guid UserId) : IRequest<Result<IEnumerable<TaskModel>>>;
}
