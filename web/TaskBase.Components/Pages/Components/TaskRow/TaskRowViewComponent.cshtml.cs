using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;

namespace TaskBase.Components.Pages.Components.TaskRow
{
    public class TaskRowViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(TaskRowCustomizationModel model)
        {
            return await Task.Factory.StartNew(() =>
            {
                return View(model);
            });
        }
    }
}
