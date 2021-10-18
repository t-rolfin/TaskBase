using System;
using MediatR;
using Rolfin.Result;
using TaskBase.Core.Enums;

namespace TaskBase.Application.Commands.ChangeTaskState
{
    public record ChangeTaskStateCommand(Guid TaskId, TaskState TaskState) 
        : IRequest<Result<bool>>;
}
