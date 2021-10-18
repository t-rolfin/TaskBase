using MediatR;
using Rolfin.Result;
using System;

namespace TaskBase.Application.Commands.SetPriorityLevel
{
    public record SetPriorityLevelCommand(int PriorityLevelKey, Guid TaskId) : IRequest<Result<bool>>;
}
