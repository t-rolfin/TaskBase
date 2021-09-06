using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Components.Models
{
    public class TaskRowModel
    {
        public TaskRowModel(IEnumerable<TaskModel> tasks, TaskRowCustomizationModel componentCustomization)
        {
            Tasks = tasks;
            ComponentCustomization = componentCustomization;
        }

        public IEnumerable<TaskModel> Tasks { get; set; }
        public TaskRowCustomizationModel ComponentCustomization { get; set; }
    }
}
