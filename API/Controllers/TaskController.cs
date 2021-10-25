using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rolfin.Result;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Commands.ChangeTaskState;
using TaskBase.Application.Commands.CreateTask;
using TaskBase.Application.Commands.DeleteTask;
using TaskBase.Application.Commands.SetPriorityLevel;
using TaskBase.Application.Commands.UpdateTask;
using TaskBase.Application.Models;
using TaskBase.Application.Queries.GetTask;
using TaskBase.Application.Queries.GetTasksByUser;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(Roles = "Member, Admin")]
    public class TaskController : BaseController
    {
        /// <summary>
        /// Returns user tasks.
        /// </summary>
        [HttpGet]
        [Route("tasks")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<TaskModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Tasks()
        {
            Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out Guid currentUserId);
            GetTasksByUserQuery query = new(currentUserId);
            return await SendWithMediator(query);
        }

        /// <summary>
        /// Returns details about a task.
        /// </summary>
        [HttpGet]
        [Route("task/{taskId:Guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TaskModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Task([FromRoute]Guid taskId, CancellationToken cancellationToken)
        {
            GetTaskQuery query = new(taskId);
            return await SendWithMediator(query, cancellationToken);
        }

        /// <summary>
        /// Create and assing a task.
        /// </summary>
        [HttpPost]
        [Route("task")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TaskModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateTask(CreateTaskCommand model, CancellationToken cancellationToken)
        {
            var dueDate = model.DueDate == default ? DateTime.Now : model.DueDate;

            var command = model with { DueDate = dueDate };

            var result = await Mediator.Send(command, cancellationToken);

            return result.IsSuccess
                ? Created("", result.Value)
                : BadRequest(result.MetaResult);
        }

        /// <summary>
        /// Delete a task from a user.
        /// </summary>
        [HttpDelete]
        [Route("task")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteTask(DeleteTaskCommand model, CancellationToken cancellationToken)
        {
            return await SendWithMediator(model, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Update a task.
        /// </summary>
        [HttpPut]
        [Route("task")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateTask(UpdateTaskCommand model, CancellationToken cancellationToken)
        {
            return await SendWithMediator(model, cancellationToken);
        }

        /// <summary>
        /// Change priority level to a task.
        /// </summary>
        [HttpPut]
        [Route("task/prioritylevel")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SetPriorityLevel(SetPriorityLevelCommand model, CancellationToken cancellationToken)
        {
            return await SendWithMediator(model, cancellationToken);
        }

        /// <summary>
        /// Change task state.
        /// </summary>
        [HttpPut]
        [Route("task/changestate")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ChangeTaskState(ChangeTaskStateCommand model, CancellationToken cancellationToken)
        {
            return await SendWithMediator(model, cancellationToken);
        }
    }
}
