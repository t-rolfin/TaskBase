using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer<TaskRowViewComponent> _stringLocalizer;

        public TaskRowViewComponent(ITaskFacade taskFacade, IStringLocalizer<TaskRowViewComponent> stringLocalizer)
        {
            _taskFacade = taskFacade;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<IViewComponentResult> InvokeAsync(TaskRowModel model)
        {
            var tasks = await _taskFacade.GetTasksAsync();

            model.Tasks = tasks?.Where(x => x.TaskState == model.RowType)
                .Select(x => {
                    return new TaskModel() { Id = x.Id, TaskTitle = x.Title, TaskDescription = x.Description, TaskState = x.TaskState };
                });

            return View(model);
        }

    }
}
