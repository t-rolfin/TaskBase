using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Application.Models;
using TaskBase.Application.Services;
using TaskBase.Components.Models;
using TaskBase.Core.Interfaces;

namespace TaskBase.Components.Views.Shared.Components.TaskNotes
{
    [ViewComponent( Name = "TaskNotes")]
    public class TaskNotesViewComponent : ViewComponent
    {
        readonly IQueryRepository _queryRepository;

        public TaskNotesViewComponent(IQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid TaskId)
        {
            if (TaskId == Guid.Empty)
                return View((Guid.NewGuid(), new List<NoteModel>()));

            var taskNotes = (await _queryRepository.GetTaskNotesAsync(TaskId.ToString(), default)).ToList();
            return View((TaskId, taskNotes));
        }
    }
}
