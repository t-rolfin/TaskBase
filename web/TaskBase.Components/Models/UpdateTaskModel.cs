using System;

namespace TaskBase.Components.Models
{
    public class UpdateTaskModel
    {
        //"taskId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        //"title": "string",
        //"description": "string"

        public UpdateTaskModel() { }

        public UpdateTaskModel(Guid taskId, string title, string description)
        {
            TaskId = taskId;
            Title = title;
            Description = description;
        }

        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
