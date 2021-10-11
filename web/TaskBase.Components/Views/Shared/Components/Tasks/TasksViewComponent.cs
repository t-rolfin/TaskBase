using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Application.Services;
using TaskBase.Components.Models;
using TaskBase.Components.Services;
using TaskBase.Core.Enums;
using TaskBase.Core.Interfaces;

namespace TaskBase.Components.Views.Shared.Components.Tasks
{
    [ViewComponent(Name = "Tasks")]
    public class TasksViewComponent : ViewComponent
    {
        readonly IIdentityService _identityProvider;
        readonly IFacade _taskFacade;

        List<TaskModel> Tasks { get; set; } = new();

        public List<TaskRowModel> Rows { get; set; } = new();

        public TasksViewComponent(IIdentityService identityProvider, IFacade taskFacade)
        {
            _identityProvider = identityProvider;
            _taskFacade = taskFacade;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUserId = _identityProvider.GetCurrentUserIdentity();
            var tasks = await _taskFacade.GetTasksByUserIdAsync(currentUserId);

            this.Tasks = tasks.Select(x => 
                new TaskModel() { 
                    Id = x.Id, 
                    TaskDescription = x.Description, 
                    TaskState = x.TaskState,    
                    TaskTitle = x.Title,
                    DueDate = x.DueDate
                })
                .ToList();

            GenerateRows();

            return View(Rows);
        }

        void GenerateRows()
        {
            Rows.Add(new TaskRowModel(
                    Guid.NewGuid(),
                    TaskState.ToDo,
                    Tasks.Where(x => x.TaskState == TaskState.ToDo),
                    new TaskRowCustomization("newTaskRow", "To Do", "bg-info")
                ));

            Rows.Add(new TaskRowModel(
                    Guid.NewGuid(),
                    TaskState.Doing,
                    Tasks.Where(x => x.TaskState == TaskState.Doing),
                    new TaskRowCustomization("inProggressTaskRow", "Doing", "bg-secondary")
                ));

            Rows.Add(new TaskRowModel(
                    Guid.NewGuid(),
                    TaskState.Done,
                    Tasks.Where(x => x.TaskState == TaskState.Done),
                    new TaskRowCustomization("completedTaskRow", "Done", "bg-success")
                ));

        }
    }
}
