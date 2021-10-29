using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Components.Models
{
    public class NoteModel
    {
        //  {
        //    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        //    "content": "string",
        //    "createdAt": "2021-10-25T07:11:16.021Z"
        //  }

        public NoteModel() { }

        public NoteModel(Guid id, string content, DateTime createdAt)
        {
            Id = id;
            Content = content;
            CreatedAt = createdAt;
        }

        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
