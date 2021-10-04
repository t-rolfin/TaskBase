using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Components.Models
{
    public record NoteModel(Guid Id, string Content, DateTime CreatedAt) { }
}
