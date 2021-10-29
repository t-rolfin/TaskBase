using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Components.Services;

namespace TaskBase.Components.Views.Shared.Components.TaskNotes
{
    [ViewComponent( Name = "TaskNotes")]
    public class TaskNotesViewComponent : ViewComponent
    {
        private readonly INoteService _noteService;

        public TaskNotesViewComponent(INoteService noteService)
        {
            _noteService = noteService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid TaskId)
        {
            if (TaskId == Guid.Empty)
                return View((Guid.NewGuid(), new List<NoteModel>()));

            var notes = await _noteService.GetNotesByTaskAsync(TaskId);

            return View((TaskId, notes.Count() != 0 ? notes.ToList() : new List<NoteModel>()));
        }
    }
}
