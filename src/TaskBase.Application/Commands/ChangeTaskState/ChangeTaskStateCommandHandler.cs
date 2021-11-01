using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Rolfin.Result;
using TaskBase.Application.Services;
using TaskBase.Core.Interfaces;

namespace TaskBase.Application.Commands.ChangeTaskState
{
    public class ChangeTaskStateCommandHandler :
        IRequestHandler<ChangeTaskStateCommand, Result<bool>>
    {
        readonly INotificationService _notificationService;
        readonly IIdentityProvider _identityService;
        readonly ILogger<ChangeTaskStateCommandHandler> _logger;
        readonly IUnitOfWork _unitOfWork;

        public ChangeTaskStateCommandHandler(INotificationService notificationService,
            IIdentityProvider identityService,
            ILogger<ChangeTaskStateCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _identityService = identityService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(ChangeTaskStateCommand request, CancellationToken cancellationToken)
        {
            var task = await _unitOfWork.Tasks.GetTaskAsync(request.TaskId);

            task.ChangeTaskState(request.TaskState);
            await _unitOfWork.Tasks.UpdateAsync(task, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"---> The state of the task: {task.Id} was changed in \"{request.TaskState}\".");

            return Result.Success(true).With($"The state of task was successfully changed in to {request.TaskState}");
        }
    }
}
