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
            TaskState = TaskState.ToDo;
            AssignTo = assignTo;
        }

        public string Title { get; protected set; }
        public string Description { get; protected set; }
        public DateTime CreatedAt { get; init; }
        public DateTime CompletedAt { get; protected set; }
        public DateTime DueDate { get; protected set; }
        public TaskState TaskState { get; protected set; }
        public User AssignTo { get; protected set; }

        public void ChangeTaskState(TaskState taskState)
        {
            if (taskState != TaskState)
                this.TaskState = taskState;
        }

        public void EditDescription(string newDescription)
        {
            _ = string.IsNullOrWhiteSpace(newDescription)
                ? throw new ArgumentNullException("The edited description must not be null or empty.")
                : this.Description = newDescription;
        }

        public void EditTitle(string newTitle)
        {
            _ = string.IsNullOrWhiteSpace(newTitle)
                ? throw new ArgumentNullException("The edited description must not be null or empty.")
                : this.Title = newTitle;
        }

    }
}
