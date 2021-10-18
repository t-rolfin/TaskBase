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
using TaskBase.Core.Enums;
using TaskBase.Core.Interfaces;
using TaskBase.Data.NotificationService;
using MediatR;
using TaskBase.Application.Commands.CreateTask;
using TaskBase.Application.Commands.ChangeTaskState;
using TaskBase.Application.Commands.DeleteTask;
using TaskBase.Application.Commands.UpdateTask;
using TaskBase.Application.Commands.CreateNote;
using TaskBase.Application.Commands.EliminateNote;
using TaskBase.Application.Commands.EditNote;
using TaskBase.Application.Commands.RemoveNotification;
using PriorityLevel = TaskBase.Core.TaskAggregate.PriorityLevel;
using TaskBase.Data.Identity;
using TaskBase.Application.Services;

namespace TaskBase.RazorPages.Pages
{
    [Authorize(Roles = "Member")]
    public class TasksModel : PageModel
    {
        readonly IMediator _mediator;
        readonly IAuthTokenFactory _tokenFactory;
        readonly IIdentityService _identityService;

        public TasksModel(IMediator mediator,
            IAuthTokenFactory tokenFactory,
            IIdentityService identityService
            )
        {
            _mediator = mediator;
            _tokenFactory = tokenFactory;
            _identityService = identityService;
        }

        public async Task OnGetAsync() 
        {
            var token = await _tokenFactory.GetToken(_identityService.GetCurrentUserIdentity());
            HttpContext.Response.Cookies.Append("jwt_token", token); 
        }

        public async Task<IActionResult> OnPostAsync(CreateTaskModel model, CancellationToken cancellationToken)
        {
            CreateTaskCommand command = new(model.Title, model.Description, model.DueDate, model.AssignTo, model.PriorityLevel);
            var result = await _mediator.Send(command, cancellationToken);

            return ViewComponent("Tasks");
        }

        public async Task OnPostChangeTaskState(Guid taskId, string newState, CancellationToken cancellationToken)
        {
            var isSuccess = Enum.TryParse(typeof(TaskState), newState, out object response);

            if (isSuccess && response is not null)
            {
                ChangeTaskStateCommand command = new(taskId, (TaskState)Enum.Parse(typeof(TaskState), newState));
                await _mediator.Send(command, cancellationToken);
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(string taskId, CancellationToken cancellationToken)
        {
            DeleteTaskCommand command = new(Guid.Parse(taskId));
            var result = await _mediator.Send(command, cancellationToken);
            return ViewComponent("Tasks");
        }

        public async Task<IActionResult> OnGetTaskDetailsAsync(string taskId, CancellationToken cancellationToken)
        {
            return await Task.FromResult(ViewComponent("TaskDetails", Guid.Parse(taskId)));
        }

        public async Task<IActionResult> OnPostUpdateTaskDescriptionAsync(string taskId, string newDescription, CancellationToken cancellationToken)
        {
            UpdateTaskCommand command = new(Guid.Parse(taskId), "", newDescription); 
            var result = await _mediator.Send(command, cancellationToken);
            return ViewComponent("Tasks");
        }

        public async Task<IActionResult> OnPostUpdateTaskTitleAsync(string taskId, string newTitle, CancellationToken cancellationToken)
        {
            UpdateTaskCommand command = new(Guid.Parse(taskId), newTitle, "");
            var result = await _mediator.Send(command, cancellationToken);
            return ViewComponent("Tasks");
        }
    
        public async Task<IActionResult> OnPostCreateNoteAsync(string taskId, string noteContent, CancellationToken cancellationToken)
        {
            CreateNoteCommand command = new(Guid.Parse(taskId), noteContent, DateTime.Now);
            var result = await _mediator.Send(command, cancellationToken);

            return ViewComponent("TaskNotes", Guid.Parse(taskId));
        }

        public async Task OnPostRemoveNoteAsync(string taskId, string noteId, CancellationToken cancellationToken)
        {
            EliminateNoteCommand command = new(Guid.Parse(taskId), Guid.Parse(noteId));
            await _mediator.Send(command, cancellationToken);
        }

        public async Task OnPostEditNoteAsync(string taskId, string noteId, string newContent, CancellationToken cancellationToken)
        {
            EditNoteCommand command = new(Guid.Parse(taskId), Guid.Parse(noteId), newContent);
            var result = await _mediator.Send(command, cancellationToken);
        }

        public async Task<IActionResult> OnPostRemoveNotificationAsync(Guid notificationId, CancellationToken cancellationToken)
        {
            RemoveNotificationCommand command = new(notificationId);
            var result = await _mediator.Send(command, cancellationToken);

            return new JsonResult(result.IsSuccess);
        }
    }
}
