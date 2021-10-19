using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Commands.ChangeTaskState;
using TaskBase.Application.Commands.CreateTask;
using TaskBase.Application.Commands.DeleteTask;
using TaskBase.Application.Commands.SetPriorityLevel;
using TaskBase.Application.Commands.UpdateTask;
using TaskBase.Application.Queries.GetTask;
using TaskBase.Application.Queries.GetTasksByUser;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(Roles = "Member, Admin")]
    public class TaskController : BaseController
    {

        [HttpGet]
        [Route("tasks")]
        public async Task<IActionResult> Tasks()
        {
            Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out Guid currentUserId);
            GetTasksByUserQuery query = new(currentUserId);
            return await SendWithMediator(query, true);
        }

        [HttpGet]
        public async Task<IActionResult> Task(GetTaskQuery model, CancellationToken cancellationToken)
        {
            return await SendWithMediator(model, true, cancellationToken);
        }

        [HttpPost]
        [Route("task/create")]
        public async Task<IActionResult> CreateTask(CreateTaskCommand model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var dueDate = model.DueDate == default ? DateTime.Now : model.DueDate;

            var command = model with { DueDate = dueDate };

            var result = await Mediator.Send(command, cancellationToken);

            return result.IsSuccess
                ? Created("", result.Value)
                : BadRequest(result.MetaResult.Message);
        }
    
        [HttpPost]
        [Route("task/delete")]
        public async Task<IActionResult> DeleteTask(DeleteTaskCommand model, CancellationToken cancellationToken)
        {
            return await SendWithMediator(model, cancellationToken: cancellationToken);
        }
    
        [HttpPost]
        [Route("task/update")]
        public async Task<IActionResult> UpdateTask(UpdateTaskCommand model, CancellationToken cancellationToken)
        {
            return await SendWithMediator(model, false, cancellationToken);
        }
    
        [HttpPost]
        [Route("task/prioritylevel")]
        public async Task<IActionResult> SetPriorityLevel(SetPriorityLevelCommand model, CancellationToken cancellationToken)
        {
            return await SendWithMediator(model, false, cancellationToken);
        }

        [HttpPost]
        [Route("task/changestate")]
        public async Task<IActionResult> ChangeTaskState(ChangeTaskStateCommand model, CancellationToken cancellationToken)
        {
            return await SendWithMediator(model, false, cancellationToken);
        }
    }
}
