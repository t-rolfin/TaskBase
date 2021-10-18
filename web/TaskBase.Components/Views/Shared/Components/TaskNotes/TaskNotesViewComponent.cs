using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Application.Models;
using TaskBase.Application.Queries.GetTaskNotes;
using TaskBase.Application.Services;
using TaskBase.Components.Models;
using TaskBase.Core.Interfaces;

namespace TaskBase.Components.Views.Shared.Components.TaskNotes
{
    [ViewComponent( Name = "TaskNotes")]
    public class TaskNotesViewComponent : ViewComponent
    {
        readonly IMediator _mediator;

        public TaskNotesViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid TaskId)
        {
            if (TaskId == Guid.Empty)
                return View((Guid.NewGuid(), new List<NoteModel>()));

            GetTaskNotesQuery query = new(TaskId);
            var result = await _mediator.Send(query);

            return View((TaskId, result.IsSuccess ? result.Value.ToList() : new List<NoteModel>()));
        }
    }
}
