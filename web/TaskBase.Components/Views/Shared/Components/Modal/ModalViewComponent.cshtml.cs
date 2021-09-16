using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Core.Interfaces;

namespace TaskBase.Components.Views.Components.Modal
{
    [ViewComponent(Name = "Modal")]
    public class ModalViewComponent : ViewComponent
    {
        private readonly ITaskFacade _taskFacade;

        public ModalViewComponent(ITaskFacade taskFacade)
        {
            _taskFacade = taskFacade;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                return View();
            });
        }
    }
}
