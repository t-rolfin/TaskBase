using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;

namespace TaskBase.Components.Views.Shared.Components.TaskDetails
{
    [ViewComponent(Name = "TaskDetails")]
    public class TaskDetailsViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync(TaskDetailsModel model)
        {
            await Task.CompletedTask;
            return View(model);
        }
    }
}
