using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Components.Services;
using TaskBase.Core.Enums;
using TaskBase.Core.Interfaces;

namespace TaskBase.Components.Views.Shared.Components.Tasks
{
    [ViewComponent(Name = "Tasks")]
    public class TasksViewComponent : ViewComponent
    {
        readonly IIdentityProvider _identityProvider;
        readonly ITaskFacade _taskFacade;

        List<TaskModel> Tasks { get; set; } = new();

        public List<TaskRowModel> Rows { get; set; } = new();

        public TasksViewComponent(IIdentityProvider identityProvider, ITaskFacade taskFacade)
        {
            _identityProvider = identityProvider;
            _taskFacade = taskFacade;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUserId = _identityProvider.GetCurrentUserIdentity();
            var tasks = await _taskFacade.GetTasksByUserIdAsync(Guid.Parse(currentUserId));

            this.Tasks = tasks.Select(x => 
                new TaskModel() { 
                    Id = x.Id, 
                    TaskDescription = x.Description, 
                    TaskState = x.TaskState,    
                    TaskTitle = x.Title 
                })
                .ToList();

            GenerateRows();

            return View(Rows);
        }

        void GenerateRows()
        {
            Rows.Add(new TaskRowModel(
                    Guid.NewGuid(),
                    TaskState.New,
                    Tasks.Where(x => x.TaskState == TaskState.New),
                    new TaskRowCustomization("newTaskRow", "To Do", "bg-info")
                ));

            Rows.Add(new TaskRowModel(
                    Guid.NewGuid(),
                    TaskState.InProgress,
                    Tasks.Where(x => x.TaskState == TaskState.InProgress),
                    new TaskRowCustomization("inProggressTaskRow", "In Progress", "bg-secondary")
                ));

            Rows.Add(new TaskRowModel(
                    Guid.NewGuid(),
                    TaskState.Completed,
                    Tasks.Where(x => x.TaskState == TaskState.Completed),
                    new TaskRowCustomization("completedTaskRow", "Done", "bg-success")
                ));

        }
    }
}
