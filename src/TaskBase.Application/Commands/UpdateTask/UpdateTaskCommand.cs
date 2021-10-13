using MediatR;
using Rolfin.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Application.Commands.UpdateTask
{
    public record UpdateTaskCommand(Guid TaskId, string Title, string Description) 
        : IRequest<Result<bool>>;
}
