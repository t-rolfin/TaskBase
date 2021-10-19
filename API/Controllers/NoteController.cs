using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Commands.CreateNote;
using TaskBase.Application.Commands.EditNote;
using TaskBase.Application.Commands.EliminateNote;
using TaskBase.Application.Queries.GetTaskNotes;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(Roles = "Admin, Member")]
    public class NoteController : BaseController
    {
        [HttpGet]
        [Route("notes")]
        public async Task<IActionResult> GetTaskNotes(GetTaskNotesQuery model, CancellationToken cancellationToken)
        {
            return await SendWithMediator(model, true, cancellationToken);
        }
    
        [HttpPost]
        [Route("note/create")]
        public async Task<IActionResult> CreateNote(CreateNoteCommand model, CancellationToken cancellationToken)
        {
            return await SendCreateWithMediator(model, cancellationToken);
        }

        [HttpPost]
        [Route("note/delete")]
        public async Task<IActionResult> DeleteNote(EliminateNoteCommand model, CancellationToken cancellationToken)
        {
            return await SendWithMediator(model, cancellationToken: cancellationToken);
        }

        [HttpPost]
        [Route("note/update")]
        public async Task<IActionResult> UpdateNote(EditNoteCommand model, CancellationToken cancellationToken)
        {
            return await SendWithMediator(model, cancellationToken: cancellationToken);
        }
    }
}
