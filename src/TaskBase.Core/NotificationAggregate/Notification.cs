using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Core.Interfaces;
using TaskBase.Core.Shared;

namespace TaskBase.Core.NotificationAggregate
{
    public class Notification : Entity<Guid>, IRootAggregate
    {
        protected Notification() : base(Guid.NewGuid()) { }

        public Notification(string title, string description, Guid userId)
            :base(Guid.NewGuid())
        {
            Title = title;
            Description = description;
            UserId = userId;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
    }
}
