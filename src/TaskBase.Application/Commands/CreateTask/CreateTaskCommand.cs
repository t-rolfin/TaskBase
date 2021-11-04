using MediatR;
using Rolfin.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Application.Models;
using TaskBase.Core.TaskAggregate;
using CoreTask = TaskBase.Core.TaskAggregate.Task;

namespace TaskBase.Application.Commands.CreateTask
{
    /// <param name="AssignTo">The user name.</param>
    public record CreateTaskCommand(
            string Title, 
            string Description, 
            DateTime DueDate, 
            string AssignTo, 
            int PriorityLevel
        ) : IRequest<TaskModelExt>;
}
