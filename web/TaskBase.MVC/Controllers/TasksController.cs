using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Components.Services;
using TaskBase.Core.Enums;
using TaskBase.Core.Interfaces;

namespace TaskBase.MVC.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {

        private readonly ITaskFacade _taskFacade;
        private readonly IIdentityProvider _identityProvider;

        public TasksController(ITaskFacade taskFacade, IIdentityProvider identityProvider)
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
            var userId = Guid.Parse(_identityProvider.GetCurrentUserIdentity());
            var userName = _identityProvider.GetCurrentUserName();
            var response = await _taskFacade.CreateTaskAsync(
                model.Title, 
                model.Description, 
                DateTime.Now.AddDays(3),
                userId,
                userName, default);

            var tasks = await _taskFacade.GetTasksByUserIdAsync(userId);

            return ViewComponent("TaskRow", new TaskRowModel(
                TaskState.New,
                tasks.Select(x =>
                {
                    return new TaskModel() { Id = x.Id, TaskTitle = x.Title, TaskDescription = x.Description, TaskState = x.TaskState };
                }),
                new TaskRowCustomizationModel(true, "Haha", "bg-info", "newTaskRow")
            ));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string taskId)
        {
            var userId = Guid.Parse(_identityProvider.GetCurrentUserIdentity());
            await _taskFacade.DeleteTaskAsync(Guid.Parse(taskId), default);

            return RedirectPermanent("/Tasks");
        }

        [HttpPost]
        public async Task<IActionResult> Done(string taskId)
        {
            await _taskFacade.CloseTaskAsync(Guid.Parse(taskId), default);

            return RedirectPermanent("/Tasks");
        }
    }
}
