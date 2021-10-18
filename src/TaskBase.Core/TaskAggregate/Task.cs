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
        List<Note> _notes = new List<Note>();

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

        public Task(string title, string description, DateTime dueDate, PriorityLevel priorityLevel, User assignTo)
            : this(title, description, dueDate, assignTo)
        {
            PriorityLevel = priorityLevel;
        }

        public string Title { get; protected set; }
        public string Description { get; protected set; }
        public DateTime CreatedAt { get; init; }
        public DateTime CompletedAt { get; protected set; }
        public DateTime DueDate { get; protected set; }
        public TaskState TaskState { get; protected set; }
        public User AssignTo { get; protected set; }
        public PriorityLevel PriorityLevel { get; protected set; }

        public IReadOnlyList<Note> Notes 
            => _notes.AsReadOnly();

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

        public Note CreateNote(string content)
        {
            Guard.Against.NullOrWhiteSpace(content, nameof(content), "Note content can not be null!");
            var note = new Note(content, DateTime.Now);
            _notes.Add(note);

            return note;
        }

        public void EditeNote(Guid noteId, string newContent)
        {
            Guard.Against.NullOrWhiteSpace(newContent, nameof(newContent), "Note content can not be null!");
            Guard.Against.NotFound(noteId.ToString(), _notes, "Id");

            var note = _notes.First(x => x.Id == noteId);
            note.Content = newContent;

            note.IsModified = true;
            note.EntityStatus = EntityStatus.Modified;
        }

        public void EliminateNote(Guid noteId)
        {
            var note = _notes.FirstOrDefault(x => x.Id == noteId);

            _ = note is default(Note)
                ? throw new NotFoundException(noteId.ToString(), nameof(Note))
                : note.EntityStatus = EntityStatus.Deleted;

            note.IsModified = true;
        }

        public void SetHighPriorityLevel()
        {
            if (PriorityLevel.Value == PriorityLevel.High.Value)
                return;

            this.PriorityLevel = PriorityLevel.High;
        }

        public void SetVeryHighPriorityLevel()
        {
            if (PriorityLevel.Value == PriorityLevel.VeryHigh.Value)
                return;

            this.PriorityLevel = PriorityLevel.VeryHigh;
        }

        public void SetLowPriorityLevel()
        {
            if (PriorityLevel.Value == PriorityLevel.Low.Value)
                return;

            this.PriorityLevel = PriorityLevel.Low;
        }
    }
}
