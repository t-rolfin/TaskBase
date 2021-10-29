using System;

namespace TaskBase.Components.Models
{
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
