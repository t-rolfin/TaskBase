using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskBase.Application.Models;
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

        readonly IQueryRepository _queryRepository;
        private readonly IIdentityService _identityProvider;

        public TasksController(IIdentityService identityProvider, IQueryRepository queryRepository)
        {
            _identityProvider = identityProvider;
            _queryRepository = queryRepository;
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
            //var response = await _taskFacade.CreateTaskAsync(
            //    model.Title, 
            //    model.Description, 
            //    DateTime.Now.AddDays(3),
            //    userId,
            //    userName, default);

            var tasks = await _queryRepository.GetTasksByUserIdAsync(userId);

            return ViewComponent("TaskRow", new TaskRowModel(
                Guid.NewGuid(),
                TaskState.ToDo,
                tasks,
                new TaskRowCustomization("newTaskRow", "To Do", "bg-info")
            ));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string taskId)
        {
            var userId = _identityProvider.GetCurrentUserIdentity();
            //await _taskFacade.DeleteTaskAsync(Guid.Parse(taskId), default);

            return RedirectPermanent("/Tasks");
        }

        [HttpPost]
        public async Task<IActionResult> Done(string taskId)
        {
            //await _taskFacade.ChangeTaskState(Guid.Parse(taskId), TaskState.Done, default);

            return RedirectPermanent("/Tasks");
        }
    }
}
