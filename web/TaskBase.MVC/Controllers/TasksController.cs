using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Core.Enums;
using TaskBase.Core.Interfaces;

namespace TaskBase.MVC.Controllers
{
    public class TasksController : Controller
    {

        private readonly ITaskFacade _taskFacade;

        public TasksController(ITaskFacade taskFacade)
        {
            _taskFacade = taskFacade;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateTaskModel model)
        {
            var response = await _taskFacade.CreateTaskAsync(model.Title, model.Description, DateTime.Now.AddDays(3), default);

            var tasks = await _taskFacade.GetTasksAsync();

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
            var response = await _taskFacade.DeleteTaskAsync(Guid.Parse(taskId), default);

            var tasks = await _taskFacade.GetTasksAsync();

            return RedirectPermanent("/Tasks");
        }

        [HttpPost]
        public async Task<IActionResult> Done(string taskId)
        {
            var response = await _taskFacade.CloseTaskAsync(Guid.Parse(taskId), default);

            var tasks = await _taskFacade.GetTasksAsync();

            return RedirectPermanent("/Tasks");
        }
    }
}
