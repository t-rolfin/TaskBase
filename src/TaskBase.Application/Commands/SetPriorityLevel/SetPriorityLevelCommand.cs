using MediatR;
using Rolfin.Result;
using System;

namespace TaskBase.Application.Commands.SetLowPriorityLevel
{
    public record SetPriorityLevelCommand(int PriorityLevelKey, Guid TaskId) : IRequest<Result<bool>>;
}
