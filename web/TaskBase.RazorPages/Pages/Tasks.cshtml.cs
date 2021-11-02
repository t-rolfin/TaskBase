using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskBase.Components.Models;
using TaskBase.Components.Services;
using Microsoft.AspNetCore.Authentication;
using TaskBase.Components.Enums;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskBase.Components.Utils;
using System.IO;

namespace TaskBase.RazorPages.Pages
{
    [Authorize(Roles = "Member")]
    public class TasksModel : PageModel
    {
        readonly ITaskService _taskService;
        readonly INoteService _noteService;
        readonly INotificationService _notificationService;

        public TasksModel(
            ITaskService taskService,
            INoteService noteService,
            INotificationService notificationService)
        {
            _taskService = taskService;
            _noteService = noteService;
            _notificationService = notificationService;
        }

        public async Task OnGetAsync() 
        {
            var token = HttpContext.Session.GetString("access_token");
            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync(CreateTaskModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return Partial("_CreationModalBody", model);

            var result = await _taskService.CreateTask(model);

            if(result.IsT1)
            {
                var resultError = result.AsT1;
                resultError.ConvertToModelStateErrors(ModelState);
                return Partial("_CreationModalBody", new CreateTaskModel());
            }
            else
                return Partial("_CreationModalBody", new CreateTaskModel());
        }

        public async Task<IActionResult> OnGetRefreshTasks()
        {
            return await Task.FromResult(ViewComponent("Tasks"));
        }

        public async Task OnPostChangeTaskState(Guid taskId, string newState, CancellationToken cancellationToken)
        {
            var isSuccess = Enum.TryParse(typeof(TaskState), newState, out object response);
            var state = (int)((TaskState)response);
            if (isSuccess && response is not null)
            {
                await _taskService.ChangeTaskState(new ChangeTaskStateModel(taskId, state));
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid taskId, CancellationToken cancellationToken)
        {
            await _taskService.DeleteTask(taskId);
            return ViewComponent("Tasks");
        }

        public async Task<IActionResult> OnGetTaskDetailsAsync(string taskId, CancellationToken cancellationToken)
        {
            return await Task.FromResult(ViewComponent("TaskDetails", Guid.Parse(taskId)));
        }

        public async Task<IActionResult> OnPostUpdateTaskDescriptionAsync(Guid taskId, string newDescription, CancellationToken cancellationToken)
        {
            await _taskService.UpdateTask(new UpdateTaskModel(taskId, "", newDescription));
            return ViewComponent("Tasks");
        }

        public async Task<IActionResult> OnPostUpdateTaskTitleAsync(Guid taskId, string newTitle, CancellationToken cancellationToken)
        {
            await _taskService.UpdateTask(new UpdateTaskModel(taskId, newTitle, ""));
            return ViewComponent("Tasks");
        }

        public async Task<IActionResult> OnPostCreateNoteAsync(string taskId, string noteContent, CancellationToken cancellationToken)
        {
            await _noteService.CreateNoteAsync(Guid.Parse(taskId), noteContent);
            return ViewComponent("TaskNotes", Guid.Parse(taskId));
        }

        public async Task OnPostRemoveNoteAsync(string taskId, string noteId, CancellationToken cancellationToken)
        {
            await _noteService.DeleteNote(Guid.Parse(taskId), Guid.Parse(noteId));
        }

        public async Task OnPostEditNoteAsync(string taskId, string noteId, string newContent, CancellationToken cancellationToken)
        {
            await _noteService.UpdateNote(Guid.Parse(taskId), Guid.Parse(noteId), newContent);
        }

        public async Task<IActionResult> OnPostRemoveNotificationAsync(Guid notificationId, CancellationToken cancellationToken)
        {
            var response = await _notificationService.DeleteNotification(notificationId);
            return new JsonResult(response);
        }
    }
}
