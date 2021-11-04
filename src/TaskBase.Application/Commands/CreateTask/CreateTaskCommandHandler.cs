using MediatR;
using Microsoft.Extensions.Logging;
using Rolfin.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Models;
using TaskBase.Application.Services;
using TaskBase.Core.Interfaces;
using TaskBase.Core.NotificationAggregate;
using TaskBase.Core.TaskAggregate;
using CoreTask = TaskBase.Core.TaskAggregate.Task;

namespace TaskBase.Application.Commands.CreateTask
{
    public class CreateTaskCommandHandler :
        IRequestHandler<CreateTaskCommand, TaskModelExt>
    {
        readonly IIdentityProvider _identityService;
        readonly ILogger<CreateTaskCommandHandler> _logger;
        readonly IUnitOfWork _unitOfWork;

        public CreateTaskCommandHandler(
            IIdentityProvider identityService,
            ILogger<CreateTaskCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<TaskModelExt> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            User user = string.IsNullOrWhiteSpace(request.AssignTo)
                ? await _unitOfWork.Tasks.GetUserByIdAsync(_identityService.GetCurrentUserIdentity())
                : await _unitOfWork.Tasks.GetUserByUserNameAsync(request.AssignTo);

            var priorityLevel = await _unitOfWork.Tasks.GetPriorityLevelAsync(request.PriorityLevel);

            var task = new CoreTask(request.Title, 
                request.Description, request.DueDate, 
                priorityLevel,
                user);

            await _unitOfWork.Tasks.AddAsync(task, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"---> A new task was created and assign to: \"{user.FullName}\"");

            return new TaskModelExt(task.Id, user.Id.ToString(),
                task.Title, task.Description, task.TaskState, task.DueDate,
                new PriorityLevelModel(task.PriorityLevel.Value, task.PriorityLevel.DisplayName), 
                new List<NoteModel>());
        }
    }
}
