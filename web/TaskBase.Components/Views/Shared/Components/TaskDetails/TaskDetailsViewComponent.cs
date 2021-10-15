using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Application.Models;
using TaskBase.Application.Services;
using TaskBase.Components.Models;
using TaskBase.Core.Enums;
using TaskBase.Core.TaskAggregate;

namespace TaskBase.Components.Views.Shared.Components.TaskDetails
{
    [ViewComponent(Name = "TaskDetails")]
    public class TaskDetailsViewComponent : ViewComponent
    {
        readonly IQueryRepository _queryRepository;

        public TaskDetailsViewComponent(IQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid TaskId)
        {
            if (TaskId == Guid.Empty)
                return View(new TaskModel(Guid.Empty, "", "", TaskState.ToDo, DateTime.Now, 
                    new PriorityLevelModel(1, ""), new List<NoteModel>()));

            var model = await _queryRepository.GetTaskAsync(TaskId);

            return View(model);
        }
    }
}
