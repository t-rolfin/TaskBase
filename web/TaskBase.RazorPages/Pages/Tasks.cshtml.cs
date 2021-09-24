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
            Guid userId = default;
            string userName = default;

            if (string.IsNullOrWhiteSpace(model.AssignTo))
            {
                userId = Guid.Parse(_identityProvider.GetCurrentUserIdentity());
                userName = _identityProvider.GetCurrentUserName();
            }
            else
            {
                var user = await _taskFacade.GetUserByNameAsync(model.AssignTo);
                userId = user.Id;
                userName = user.FullName;
            }

            var response = await _taskFacade.CreateTaskAsync(
                model.Title,
                model.Description,
                model.DueDate == default ? DateTime.Now : model.DueDate ,
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
            await _taskFacade.SetTaskAsInProgressAsync(Guid.Parse(taskId), default);
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
