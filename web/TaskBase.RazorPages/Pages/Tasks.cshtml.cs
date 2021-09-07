using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskBase.Components.Models;
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

        public void OnGet()
        { }

        public async Task<IActionResult> OnPostAsync(CreateTaskModel model)
        {
            var response = await _taskFacade.CreateTaskAsync(model.Title, model.Description, DateTime.Now.AddDays(3), default);
            var tasks = new List<TaskModel>();
            tasks.Add(new TaskModel() { TaskTitle = response.Title, TaskDescription = response.Description });

            return ViewComponent("TaskRow", new TaskRowModel(
                tasks,
                new TaskRowCustomizationModel(true, "Haha", "bg-info", "newTaskRow")
            ));
        }

    }
}
