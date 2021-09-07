using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Core.Interfaces;

namespace TaskBase.Components.Pages.Components.TaskRow
{
    public class TaskRowViewComponent : ViewComponent
    {
        private readonly ITaskFacade _taskFacade;

        public TaskRowViewComponent(ITaskFacade taskFacade)
        {
            _taskFacade = taskFacade;
        }

        public async Task<IViewComponentResult> InvokeAsync(TaskRowModel model)
        {
            if (model.Tasks != null && model.Tasks.Count() != 0)
                return View(model);

            var tasks = await _taskFacade.GetTasksAsync();

            model.Tasks = tasks?.Where(x => x.TaskState == Core.Enums.TaskState.New)
                .Select(x => {
                    return new TaskModel() { TaskTitle = x.Title, TaskDescription = x.Description };
                });

            return View(model);
        }

    }
}
