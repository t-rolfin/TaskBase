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
using TaskBase.Core.Interfaces;
using TaskBase.Data.Identity;
using BaseTask = TaskBase.Core.TaskAggregate.Task;
using TaskUser = TaskBase.Core.TaskAggregate.User;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        readonly ITaskAsyncRepository _taskRepository;
        readonly UserManager<User> _userManager;

        public TaskController(ITaskAsyncRepository taskRepository, UserManager<User> userManager)
        {
            _taskRepository = taskRepository;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("Tasks")]
        public async Task<IActionResult> Tasks()
        {
            Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out Guid currentUserId);
            var currentUserTasks = await _taskRepository.GetTasksByUserAsync(currentUserId);

            return Ok(currentUserTasks);
        }

        [HttpPost]
        [Route("CreateTask")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> CreateTask(CreateTaskModel taskModel, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var assignToUserId = string.IsNullOrWhiteSpace(taskModel.UserId) || taskModel.UserId == "string"
                    ? User.FindFirst(ClaimTypes.NameIdentifier).Value
                    : taskModel.UserId;

                var dueDate = taskModel.DueDate == default ? DateTime.Now : taskModel.DueDate;

                var user = await _taskRepository.GetUserByIdAsync(Guid.Parse(assignToUserId));

                BaseTask result = default;

                if(user is not null)
                {
                    var newTask = new BaseTask(taskModel.Title, taskModel.Description, dueDate, user);
                    result = await _taskRepository.AddAsync(newTask, cancellationToken);

                }
                else
                {
                    var identityUser = await _userManager.FindByIdAsync(assignToUserId);
                    var newTask = new BaseTask(taskModel.Title, taskModel.Description, dueDate, new TaskUser(Guid.Parse(identityUser.Id), identityUser.UserName));
                    result = await _taskRepository.AddAsync(newTask, cancellationToken);
                }

                if(result is not null)
                    return Created("", result);
            }

            return BadRequest(ModelState.Values.SelectMany(x => x.Errors));
        }
    }
}
