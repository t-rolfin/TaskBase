using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Components.Services;
using TaskBase.Core.Interfaces;

namespace TaskBase.Components.Views.Components.TaskRow
{
    public class TaskRowViewComponent : ViewComponent
    {
        private readonly ITaskFacade _taskFacade;
        private readonly IIdentityProvider _identityProvider;

        public TaskRowViewComponent(ITaskFacade taskFacade, IIdentityProvider identityProvider)
        {
            _taskFacade = taskFacade;
            _identityProvider = identityProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync(TaskRowModel model)
        {
            var userId = _identityProvider.GetCurrentUserIdentity();
            var tasks = await _taskFacade.GetTasksByUserIdAsync(Guid.Parse(userId));

            model.Tasks = tasks?.Where(x => x.TaskState == model.RowType)
                .Select(x => {
                    return new TaskModel() { Id = x.Id, TaskTitle = x.Title, TaskDescription = x.Description, TaskState = x.TaskState };
                });

            return View(model);
        }

    }
}
