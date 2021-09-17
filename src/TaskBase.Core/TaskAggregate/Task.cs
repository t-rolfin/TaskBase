using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Core.Enums;
using TaskBase.Core.Interfaces;
using TaskBase.Core.Shared;

namespace TaskBase.Core.TaskAggregate
{
    public class Task : Entity<Guid>, IRootAggregate
    {
        private Task() : base(Guid.NewGuid()) { }

        public Task(string title, string description, DateTime dueDate, User assignTo)
            : base(Guid.NewGuid())
        {
            Title = Guard.Against.NullOrWhiteSpace(title, nameof(title));
            Description = description;
            DueDate = dueDate;
            CreatedAt = DateTime.Now;
            TaskState = TaskState.New;
            AssignTo = assignTo;
        }

        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime CompletedAt { get; protected set; }
        public DateTime DueDate { get; protected set; }
        public TaskState TaskState { get; protected set; }
        public User AssignTo { get; protected set; }

        public void StartWorking()
        {
            TaskState = TaskState.InProgress;
        }

        public void CompleteTask()
        {
            TaskState = TaskState.Completed;
            CompletedAt = DateTime.Now;
        }

    }
}
