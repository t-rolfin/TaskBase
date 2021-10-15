using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Core.Enums;

namespace TaskBase.Application.Models
{
    public class TaskModel
    {
        public TaskModel() { Notes = new List<NoteModel>(); }

        public TaskModel(Guid id, 
            string title, string description, 
            TaskState taskState, DateTime dueDate, 
            PriorityLevelModel priorityLevel, List<NoteModel> notes = null)
        {
            Id = id;
            Title = title;
            Description = description;
            TaskState = taskState;
            DueDate = dueDate;
            PriorityLevel = priorityLevel;
            Notes = notes;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskState TaskState { get; set; }
        public DateTime DueDate { get; set; }
        public PriorityLevelModel PriorityLevel { get; set; } 
        public List<NoteModel> Notes { get; set; }
    }

    public class PriorityLevelModel
    {
        public PriorityLevelModel() { }

        public PriorityLevelModel(int id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }

        public int Id { get; set; } 
        public string DisplayName { get; set; }
    }

    public record NoteModel
    {
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

    public record NotificationsModel(List<NotificationModel> Notifications, int NotificationCount);
    public class NotificationModel
    {
        public NotificationModel() { }

        public NotificationModel(Guid id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

}
