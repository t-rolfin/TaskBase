using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TaskBase.Components.Models;
using TaskBase.Core.Enums;
using TaskBase.Core.Interfaces;

namespace TaskBase.RazorPages.Pages
{
    public class TasksModel : PageModel
    {
        private readonly ITaskFacade _taskFacade;
        private static readonly ILog log = LogManager.GetLogger(typeof(TasksModel));

        public TasksModel(ITaskFacade taskFacade)
        {
            _taskFacade = taskFacade;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(CreateTaskModel model)
        {
            var response = await _taskFacade.CreateTaskAsync(model.Title, model.Description, DateTime.Now.AddDays(3), default);

            if (response is not null)
                log.Info("An new tass was created!");
            else
                log.Error("An error occur at task creation process!");

            var tasks = await _taskFacade.GetTasksAsync();

            return ViewComponent("TaskRow", new TaskRowModel(
                TaskState.New,
                tasks.Select(x => { 
                    return new TaskModel() { Id = x.Id, TaskTitle = x.Title, TaskDescription = x.Description, TaskState = x.TaskState }; 
                }),
                new TaskRowCustomizationModel(true, "Haha", "bg-info", "newTaskRow")
            ));
        }

        
        public async Task<IActionResult> OnPostDeleteAsync([FromForm] string taskId)
        {
            var response = await _taskFacade.DeleteTaskAsync(Guid.Parse(taskId), default);

            var tasks = await _taskFacade.GetTasksAsync();

            return RedirectPermanent("/Tasks");
        }

        public async Task<IActionResult> OnPostDoneAsync([FromForm]string taskId)
        {
            var response = await _taskFacade.CloseTaskAsync(Guid.Parse(taskId), default);

            var tasks = await _taskFacade.GetTasksAsync();

            return RedirectPermanent("/Tasks");
        }
    }
}
