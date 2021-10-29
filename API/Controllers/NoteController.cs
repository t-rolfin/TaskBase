using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rolfin.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Commands.CreateNote;
using TaskBase.Application.Commands.EditNote;
using TaskBase.Application.Commands.EliminateNote;
using TaskBase.Application.Models;
using TaskBase.Application.Queries.GetTaskNotes;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(Roles = "Admin, Member")]
    public class NoteController : BaseController
    {

        /// <summary>
        /// Get a list of notes for the specified task.
        /// </summary>
        [HttpGet]
        [Route("notes/{taskId:Guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<NoteModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTaskNotes([FromRoute]Guid taskId, CancellationToken cancellationToken)
        {
            GetTaskNotesQuery query = new(taskId);
            return await SendWithMediator(query, cancellationToken);
        }

        /// <summary>
        /// Create note for a task.
        /// </summary>
        [HttpPost]
        [Route("note")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(NoteModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateNote(CreateNoteCommand model, CancellationToken cancellationToken)
        {
            return await SendCreateWithMediator(model, cancellationToken);
        }

        /// <summary>
        /// Delete a note from a task.
        /// </summary>
        [HttpDelete]
        [Route("note/{TaskId:Guid}/{NoteId:Guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteNote([FromRoute] EliminateNoteCommand model, CancellationToken cancellationToken)
        {
            return await SendWithMediator(model, cancellationToken: cancellationToken);
        }


        /// <summary>
        /// Update content of a note.
        /// </summary>
        [HttpPut]
        [Route("note")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IMetaResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateNote(EditNoteCommand model, CancellationToken cancellationToken)
        {
            return await SendWithMediator(model, cancellationToken: cancellationToken);
        }
    }
}
