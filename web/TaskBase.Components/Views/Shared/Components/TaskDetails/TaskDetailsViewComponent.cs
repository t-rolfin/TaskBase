using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Components.Services;

namespace TaskBase.Components.Views.Shared.Components.TaskDetails
{
    [ViewComponent(Name = "TaskDetails")]
    public class TaskDetailsViewComponent : ViewComponent
    {
        readonly ITaskService _taskService;

        public TaskDetailsViewComponent(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid TaskId)
        {
            if (TaskId == Guid.Empty)
                return View(TaskModel.EmptyModel);

            var result = await _taskService.GetTask(TaskId);

            return View(result is not null ? result : TaskModel.EmptyModel);
        }
    }
}
