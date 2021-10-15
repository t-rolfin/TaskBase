using MediatR;
using Rolfin.Result;
using System;
using TaskBase.Application.Models;

namespace TaskBase.Application.Queries.GetTask
{
    public record GetTaskQuery(Guid TaskId) : IRequest<Result<TaskModel>>;
}
