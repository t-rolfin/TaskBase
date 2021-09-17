using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Core.Shared;
using Ardalis.GuardClauses;

namespace TaskBase.Core.TaskAggregate
{
    public class User : Entity<Guid>
    {
        private User() : base(Guid.NewGuid()) { } 

        public User(Guid userId, string fullName)
            : base(Guard.Against.InvalidInput(userId, nameof(userId), (id) => { return id != Guid.Empty; }))
        {
            FullName = Guard.Against.NullOrWhiteSpace(fullName, nameof(fullName));
        }

        public string FullName { get; set; }
    }
}
