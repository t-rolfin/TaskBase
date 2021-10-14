using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Core.Enums;

namespace TaskBase.Components.Models
{
    public class TaskModel
    {
        public Guid Id { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public TaskState TaskState { get; set; }
        public DateTime DueDate { get; set; }
        public PriorityLevelModel PriorityLevel { get; set; }
    }

    public record PriorityLevelModel(int key, string DisplayName);
}
