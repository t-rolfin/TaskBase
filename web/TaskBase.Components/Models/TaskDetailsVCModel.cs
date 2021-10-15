using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Components.Models
{
    public class TaskDetailsVCModel
    {
        public TaskDetailsVCModel(Guid taskId)
        {
            TaskId = taskId;
        }

        public Guid TaskId { get; set; }
    }
}
