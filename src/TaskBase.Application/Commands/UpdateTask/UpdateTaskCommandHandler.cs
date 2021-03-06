using MediatR;
using Microsoft.Extensions.Logging;
using Rolfin.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Services;
using TaskBase.Core.Interfaces;

namespace TaskBase.Application.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Result<bool>>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly ILogger<UpdateTaskCommandHandler> _logger;
        readonly INotificationService _notificationService;
        readonly IIdentityProvider _identityService;

        public UpdateTaskCommandHandler(IUnitOfWork unitOfWork,
            ILogger<UpdateTaskCommandHandler> logger,
            INotificationService notificationService,
            IIdentityProvider identityService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _notificationService = notificationService;
            _identityService = identityService;
        }

        public async Task<Result<bool>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _unitOfWork.Tasks.GetTaskAsync(request.TaskId);

            if (!string.IsNullOrWhiteSpace(request.Description)) task.EditDescription(request.Description);

            if (!string.IsNullOrWhiteSpace(request.Title)) task.EditTitle(request.Title);

            await _unitOfWork.Tasks.UpdateAsync(task, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"The task with ID: {task.Id} was successfully updated.");

            return Result<bool>.Success().With("Description for the task was changed!");
        }
    }
}
