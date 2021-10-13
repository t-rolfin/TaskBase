using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Core.Enums;
using TaskBase.Core.Shared;

namespace TaskBase.Core.TaskAggregate
{
    public class Note : Entity<Guid>
    {
        protected Note() : base(Guid.NewGuid()) { }

        public Note(string content, DateTime addedAt)
            :base(Guid.NewGuid())
        {
            Content = content;
            AddedAt = addedAt;
            EntityStatus = EntityStatus.Added;
            IsModified = true;
        }

        public string Content { get; set; }
        public DateTime AddedAt { get; set; }

        public EntityStatus EntityStatus { get; set; }
        public bool IsModified { get; set; }
    }
}
