using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Core.Interfaces;

namespace TaskBase.Components.Views.Shared.Components.TaskNotes
{
    [ViewComponent( Name = "TaskNotes")]
    public class TaskNotesViewComponent : ViewComponent
    {
        readonly IFacade _taskFacade;

        public TaskNotesViewComponent(IFacade taskFacade)
        {
            _taskFacade = taskFacade;
        }

        public async Task<IViewComponentResult> InvokeAsync(TaskNoteId idModel)
        {
            if (string.IsNullOrWhiteSpace(idModel.Id))
                return View((Guid.NewGuid().ToString(), new List<NoteModel>()));

            var taskNotes = await _taskFacade.GetTaskNotesAsync(idModel.Id, default);
            var model = taskNotes.Select(x => new NoteModel(x.Id, x.Content, x.AddedAt)).OrderByDescending(x => x.CreatedAt).ToList();
            return View((idModel.Id, model));
        }
    }
}
