using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Core.Enums;

namespace TaskBase.Components.Models
{
    public class TaskRowModel
    {
        public TaskRowModel() { }

        public TaskRowModel(TaskState rowType, IEnumerable<TaskModel> tasks, TaskRowCustomizationModel componentCustomization)
        {
            RowType = rowType;
            Tasks = tasks;
            ComponentCustomization = componentCustomization;
        }

        public IEnumerable<TaskModel> Tasks { get; set; }
        public TaskRowCustomizationModel ComponentCustomization { get; set; }
        public TaskState RowType { get; set; }
    }
}
