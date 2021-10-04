﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Core.TaskAggregate;
using CoreTask = TaskBase.Core.TaskAggregate.Task;

namespace TaskBase.Core.Interfaces
{
    public interface ITaskAsyncRepository : IAsyncRepository<CoreTask, Guid>
    {
        Task<IEnumerable<Note>> GetTaskNotesAsync(Guid taskId, CancellationToken cancellationToken);
    }
}
