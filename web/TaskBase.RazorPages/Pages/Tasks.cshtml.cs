using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        readonly IFacade _taskFacade;
        readonly IIdentityProvider _identityProvider;
        readonly ILog log = LogManager.GetLogger(typeof(TasksModel));

        public TasksModel(IFacade taskFacade, IIdentityProvider identityProvider)
        {
            _taskFacade = taskFacade;
            _identityProvider = identityProvider;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(CreateTaskModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
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

                var cookie = HttpContext.Request.Cookies.Where(x => x.Key == "auth_cookie").First();

                var response = await _taskFacade.CreateTaskAsync(
                    model.Title,
                    model.Description,
                    model.DueDate == default ? DateTime.Now : model.DueDate,
                    userId,
                    userName,
                    cancellationToken);

                if (response is not null)
                    log.Info("An new task was created!");
                else
                    log.Error("An error occur at task creation process!");

                return ViewComponent("Tasks");
            }

            return ViewComponent("Tasks");
        }

        public async Task OnPostChangeTaskState(Guid taskId, string newState, CancellationToken cancellationToken)
        {
            var isSuccess = Enum.TryParse(typeof(TaskState), newState, out object response);

            if (isSuccess && response is not null)
            {
                await _taskFacade.ChangeTaskState(taskId, (TaskState)response , cancellationToken);
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(string taskId, CancellationToken cancellationToken)
        {
            await _taskFacade.DeleteTaskAsync(Guid.Parse(taskId), cancellationToken);
            return ViewComponent("Tasks");
        }

        public async Task<IActionResult> OnGetTaskDetailsAsync(string taskId, CancellationToken cancellationToken)
        {
            var taskDetails = await _taskFacade.GetTaskDetailsAsync(Guid.Parse(taskId));
            var taskDetailsModel = new TaskDetailsModel(taskDetails.Id.ToString(), taskDetails.Title, taskDetails.Description);

            return ViewComponent("TaskDetails", taskDetailsModel);
        }

        public async Task<IActionResult> OnPostUpdateTaskDescriptionAsync(string taskId, string newDescription, CancellationToken cancellationToken)
        {
            var result = await _taskFacade.EditDescriptionAsync(taskId, newDescription, cancellationToken);
            return ViewComponent("Tasks");
        }

        public async Task<IActionResult> OnPostUpdateTaskTitleAsync(string taskId, string newTitle, CancellationToken cancellationToken)
        {
            var result = await _taskFacade.EditTitleAsync(taskId, newTitle, cancellationToken);
            return ViewComponent("Tasks");
        }
    
        public async Task<IActionResult> OnPostCreateNoteAsync(string taskId, string noteContent, CancellationToken cancellationToken)
        {
            var response = await _taskFacade.CreateNoteAsync(taskId, noteContent, cancellationToken);

            if (response != default)
                log.Info($"An new note was added to the task: { taskId }");

            return ViewComponent("TaskNotes", new TaskNoteId(taskId));
        }

        public async Task OnPostRemoveNoteAsync(string taskId, string noteId, CancellationToken cancellationToken)
        {
            await _taskFacade.EliminateNoteFromTaskAsync(taskId, noteId, cancellationToken);
        }

        public async Task OnPostEditNoteAsync(string taskId, string noteId, string newContent, CancellationToken cancellationToken)
        {
            await _taskFacade.EditNoteAsync(taskId, noteId, newContent, cancellationToken);
            log.Info($"The content of note:{ noteId } from task: { taskId } was changed!");
        }

        public async Task<IActionResult> OnPostRemoveNotificationAsync(Guid notificationId, CancellationToken cancellationToken)
        {
            var isSuccess = await _taskFacade.RemoveNotification(notificationId, cancellationToken);

            return new JsonResult(isSuccess);
        }
    }
}
