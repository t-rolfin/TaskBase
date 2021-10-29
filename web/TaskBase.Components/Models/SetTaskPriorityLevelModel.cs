using System;

namespace TaskBase.Components.Models
{
    public class SetTaskPriorityLevelModel
    {
        //"priorityLevelKey": 0,
        //"taskId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"

        public SetTaskPriorityLevelModel() { }

        public SetTaskPriorityLevelModel(Guid taskId, int priorityLevelKey)
        {
            TaskId = taskId;
            PriorityLevelKey = priorityLevelKey;
        }

        public Guid TaskId { get; set; }
        public int PriorityLevelKey { get; set; }
    }
}
