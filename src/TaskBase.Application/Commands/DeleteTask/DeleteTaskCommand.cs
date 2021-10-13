using MediatR;
using Rolfin.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Application.Commands.DeleteTask
{
    public record DeleteTaskCommand(Guid TaskId) : IRequest<Result<bool>>;
}
