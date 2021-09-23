using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TaskBase.Components.Models;
using TaskBase.Components.Services;
using TaskBase.Core.Enums;
using TaskBase.Core.Interfaces;
using BaseTask = TaskBase.Core.TaskAggregate.Task;

namespace TaskBase.RazorPages.Pages
{
    [Authorize(Roles = "Member")]
    public class TasksModel : PageModel
    {
        readonly ITaskFacade _taskFacade;
        readonly IIdentityProvider _identityProvider;
        readonly ILog log = LogManager.GetLogger(typeof(TasksModel));

        public TasksModel(ITaskFacade taskFacade, IIdentityProvider identityProvider)
        {
            _taskFacade = taskFacade;
            _identityProvider = identityProvider;
        }

        public void OnGet() { }


        public async Task<IActionResult> OnPostAsync(CreateTaskModel model)
        {
            var userId = Guid.Parse(_identityProvider.GetCurrentUserIdentity());
            var userName = _identityProvider.GetCurrentUserName();

            var response = await _taskFacade.CreateTaskAsync(
                model.Title, 
                model.Description, 
                DateTime.Now.AddDays(3), 
                userId, 
                userName, default);

            if (response is not null)
                log.Info("An new task was created!");
            else
                log.Error("An error occur at task creation process!");


            return ViewComponent("Tasks");
        }

        public async Task<IActionResult> OnPostInProgressAsync(string taskId)
        {
            await _taskFacade.DeleteTaskAsync(Guid.Parse(taskId), default);
            return ViewComponent("Tasks");
        }

        public async Task<IActionResult> OnPostDeleteAsync(string taskId)
        {
            await _taskFacade.DeleteTaskAsync(Guid.Parse(taskId), default);
            return ViewComponent("Tasks");
        }

        public async Task<IActionResult> OnPostDoneAsync(string taskId)
        {
            await _taskFacade.CloseTaskAsync(Guid.Parse(taskId), default);
            return ViewComponent("Tasks");
        }

    }
}
