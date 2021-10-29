using System;
using System.Collections.Generic;

namespace TaskBase.Components.Models
{
    public class TaskModel
    {
        //"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        //"title": "string",
        //"description": "string",
        //"taskState": 0,
        //"dueDate": "2021-10-25T07:11:16.021Z",
        //"priorityLevel" : {}
        //"notes": []

        public TaskModel() { }

        public TaskModel(Guid id, string title, string description, int taskState, DateTime dueDate,
            TaskPriorityLevelModel priorityLevel, IEnumerable<NoteModel> notes = null)
        {
            Id = id;
            Title = title;
            Description = description;
            TaskState = taskState;
            DueDate = dueDate;
            PriorityLevel = priorityLevel;
            Notes = notes ?? new List<NoteModel>();
        }

        public static TaskModel EmptyModel
            => new TaskModel(Guid.Empty, "", "", 0, DateTime.Now,
                new TaskPriorityLevelModel(1, ""), new List<NoteModel>());

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TaskState { get; set; }
        public DateTime DueDate { get; set; }


        public TaskPriorityLevelModel PriorityLevel { get; set; }
        public IEnumerable<NoteModel> Notes { get; set; }
    }
}
