using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Enums;
using TaskBase.Components.Services;

namespace TaskBase.Components.Models
{
    public class TaskRowModel
    {
        public TaskRowModel() { }

        public TaskRowModel(Guid id, TaskState rowType, IEnumerable<TaskModel> tasks, TaskRowCustomization customize)
        {
            Id = id;
            RowType = rowType;
            Tasks = tasks;
            Customize = customize;
        }

        public Guid Id { get; set; }
        public IEnumerable<TaskModel> Tasks { get; set; }
        public TaskRowCustomization Customize { get; set; }
        public TaskState RowType { get; set; }
    }
}
