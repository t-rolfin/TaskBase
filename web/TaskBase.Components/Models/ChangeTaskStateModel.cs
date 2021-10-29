using System;

namespace TaskBase.Components.Models
{
    public class ChangeTaskStateModel
    {
        //"taskId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        //"taskState": 0

        public ChangeTaskStateModel() { }

        public ChangeTaskStateModel(Guid taskId, int taskState)
        {
            TaskId = taskId;
            TaskState = taskState;
        }

        public Guid TaskId { get; set; }
        public int TaskState { get; set; }
    }
}
