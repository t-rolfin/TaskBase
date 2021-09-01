using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTask = TaskBase.Core.TaskAggregate.Task;

namespace TaskBase.Core.Interfaces
{
    public interface ITaskAsyncRepository : IAsyncRepository<CoreTask, Guid>
    { }
}
