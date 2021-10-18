using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Application.Models;
using TaskBase.Application.Queries.GetTasksByUser;
using TaskBase.Application.Services;
using TaskBase.Components.Models;
using TaskBase.Components.Services;
using TaskBase.Core.Enums;
using TaskBase.Core.Interfaces;
using TaskBase.Core.TaskAggregate;

namespace TaskBase.Components.Views.Shared.Components.Tasks
{
    [ViewComponent(Name = "Tasks")]
    public class TasksViewComponent : ViewComponent
    {
        List<TaskModel> Tasks { get; set; } = new();

        readonly IIdentityService _identityProvider;
        readonly IMediator _mediator;

        public List<TaskRowModel> Rows { get; set; } = new();

        public TasksViewComponent(IIdentityService identityProvider, IMediator mediator)
        {
            _identityProvider = identityProvider;
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUserId = _identityProvider.GetCurrentUserIdentity();

            GetTasksByUserQuery query = new(currentUserId);
            var result = await _mediator.Send(query);

            if (result.IsSuccess) this.Tasks = result.Value.ToList();

            GenerateRows();

            return View(Rows);
        }

        void GenerateRows()
        {
            Rows.Add(new TaskRowModel(
                    Guid.NewGuid(),
                    TaskState.ToDo,
                    Tasks.Where(x => x.TaskState == TaskState.ToDo).OrderByDescending(x => x.PriorityLevel.Id),
                    new TaskRowCustomization("newTaskRow", "To Do", "bg-info")
                ));

            Rows.Add(new TaskRowModel(
                    Guid.NewGuid(),
                    TaskState.Doing,
                    Tasks.Where(x => x.TaskState == TaskState.Doing).OrderByDescending(x => x.PriorityLevel.Id),
                    new TaskRowCustomization("inProggressTaskRow", "Doing", "bg-secondary")
                ));

            Rows.Add(new TaskRowModel(
                    Guid.NewGuid(),
                    TaskState.Done,
                    Tasks.Where(x => x.TaskState == TaskState.Done).OrderByDescending(x => x.PriorityLevel.Id),
                    new TaskRowCustomization("completedTaskRow", "Done", "bg-success")
                ));

        }
    }
}
