using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Commands.CreateTask;
using TaskBase.Application.Queries.GetTaskNotes;
using TaskBase.Application.Queries.GetTasksByUser;
using TaskBase.Core.Interfaces;
using TaskBase.Data.Identity;
using BaseTask = TaskBase.Core.TaskAggregate.Task;
using TaskUser = TaskBase.Core.TaskAggregate.User;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : BaseController
    {

        [HttpGet]
        [Route("Tasks")]
        public async Task<IActionResult> Tasks()
        {
            Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out Guid currentUserId);

            GetTasksByUserQuery query = new(currentUserId);
            var result = await Mediator.Send(query);

            return Ok(result.Value.ToList());
        }

        [HttpPost]
        [Route("CreateTask")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> CreateTask(CreateTaskCommand model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var dueDate = model.DueDate == default ? DateTime.Now : model.DueDate;

            var command = model with { DueDate = dueDate };

            var result = await Mediator.Send(command);

            if(result.IsSuccess)
                return Created("", result.Value);
            else
                return BadRequest(result.MetaResult.Message);
        }
    }
}
