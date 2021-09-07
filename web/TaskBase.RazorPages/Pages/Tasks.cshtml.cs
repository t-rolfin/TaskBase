using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskBase.Components.Models;
using TaskBase.Core.Enums;
using TaskBase.Core.Interfaces;

namespace TaskBase.RazorPages.Pages
{
    public class TasksModel : PageModel
    {
        private readonly ITaskFacade _taskFacade;

        public TasksModel(ITaskFacade taskFacade)
        {
            _taskFacade = taskFacade;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(CreateTaskModel model)
        {
            var response = await _taskFacade.CreateTaskAsync(model.Title, model.Description, DateTime.Now.AddDays(3), default);
            var tasks = await _taskFacade.GetTasksAsync();

            return ViewComponent("TaskRow", new TaskRowModel(
                TaskState.New,
                tasks.Select(x => { return new TaskModel() { TaskTitle = x.Title, TaskDescription = x.Description }; }),
                new TaskRowCustomizationModel(true, "Haha", "bg-info", "newTaskRow")
            ));
        }

    }
}
