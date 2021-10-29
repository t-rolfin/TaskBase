using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;

namespace TaskBase.Components.Services
{
    public interface INoteService
    {
        Task<NoteModel> CreateNoteAsync(Guid taskId, string content);
        Task<IEnumerable<NoteModel>> GetNotesByTaskAsync(Guid taskId);
        Task UpdateNote(Guid taskId, Guid noteId, string content);
        Task DeleteNote(Guid taskId, Guid noteId);
    }
}
