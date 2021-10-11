using MediatR;
using Rolfin.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTask = TaskBase.Core.TaskAggregate.Task;

namespace TaskBase.Application.Commands.CreateTask
{
    public record CreateTaskCommand([Required] string Title, [Required] string Description, DateTime DueDate, string AssignTo) 
        : IRequest<Result<CoreTask>>;
}
