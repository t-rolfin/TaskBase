using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBase.Components.Models;

namespace TaskBase.Components.Services
{
    public interface ITaskService
    {
        Task ChangeTaskState(ChangeTaskStateModel model);
        Task<TaskModel> CreateTask(CreateTaskModel model);
        Task DeleteTask(Guid TaskId);
        Task<TaskModel> GetTask(Guid taskId);
        Task<IEnumerable<TaskModel>> GetTasks();
        Task SetTaskPriorityLevel(SetTaskPriorityLevelModel model);
        Task UpdateTask(UpdateTaskModel model);
    }
}