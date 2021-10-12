using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskBase.Application.Services;
using TaskBase.Components.Models;
using TaskBase.Components.Services;
using TaskBase.Core.Enums;
using TaskBase.Core.Interfaces;

namespace TaskBase.MVC.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {

        private readonly IFacade _taskFacade;
        private readonly IIdentityService _identityProvider;

        public TasksController(IFacade taskFacade, IIdentityService identityProvider)
        {
            _taskFacade = taskFacade;
            _identityProvider = identityProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateTaskModel model)
        {
            var userId = _identityProvider.GetCurrentUserIdentity();
            var userName = _identityProvider.GetCurrentUserName();
            var response = await _taskFacade.CreateTaskAsync(
                model.Title, 
                model.Description, 
                DateTime.Now.AddDays(3),
                userId,
                userName, default);

            var tasks = await _taskFacade.GetTasksByUserIdAsync(userId);

            return ViewComponent("TaskRow", new TaskRowModel(
                Guid.NewGuid(),
                TaskState.ToDo,
                tasks.Select(x =>
                {
                    return new TaskModel() { Id = x.Id, TaskTitle = x.Title, TaskDescription = x.Description, TaskState = x.TaskState };
                }),
                new TaskRowCustomization("newTaskRow", "To Do", "bg-info")
            ));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string taskId)
        {
            var userId = _identityProvider.GetCurrentUserIdentity();
            await _taskFacade.DeleteTaskAsync(Guid.Parse(taskId), default);

            return RedirectPermanent("/Tasks");
        }

        [HttpPost]
        public async Task<IActionResult> Done(string taskId)
        {
            await _taskFacade.ChangeTaskState(Guid.Parse(taskId), TaskState.Done, default);

            return RedirectPermanent("/Tasks");
        }
    }
}
