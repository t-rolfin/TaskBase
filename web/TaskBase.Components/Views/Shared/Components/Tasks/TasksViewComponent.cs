using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Enums;
using TaskBase.Components.Models;
using TaskBase.Components.Services;

namespace TaskBase.Components.Views.Shared.Components.Tasks
{
    [ViewComponent(Name = "Tasks")]
    public class TasksViewComponent : ViewComponent
    {
        List<TaskModel> Tasks { get; set; } = new();
        public List<TaskRowModel> Rows { get; set; } = new();

        private readonly ITaskService _taskService;

        public TasksViewComponent(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _taskService.GetTasks();
            this.Tasks = result.ToList();

            GenerateRows();

            return View(Rows);
        }

        void GenerateRows()
        {
            Rows.Add(new TaskRowModel(
                    Guid.NewGuid(),
                    TaskState.ToDo,
                    Tasks.Where(x => x.TaskState == (int)TaskState.ToDo)
                        .OrderByDescending(x => x.PriorityLevel.Id),
                    new TaskRowCustomization("newTaskRow", "To Do", "bg-info")
                ));

            Rows.Add(new TaskRowModel(
                    Guid.NewGuid(),
                    TaskState.Doing,
                    Tasks.Where(x => x.TaskState == (int)TaskState.Doing)
                        .OrderByDescending(x => x.PriorityLevel.Id),
                    new TaskRowCustomization("inProggressTaskRow", "Doing", "bg-secondary")
                ));

            Rows.Add(new TaskRowModel(
                    Guid.NewGuid(),
                    TaskState.Done,
                    Tasks.Where(x => x.TaskState == (int)TaskState.Done)
                        .OrderByDescending(x => x.PriorityLevel.Id),
                    new TaskRowCustomization("completedTaskRow", "Done", "bg-success")
                ));

        }
    }
}
