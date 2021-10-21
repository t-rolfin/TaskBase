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

namespace TaskBase.Application.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler :
        IRequestHandler<DeleteTaskCommand, Result<bool>>
    {
        readonly INotificationService _notificationService;
        readonly IIdentityService _identityService;
        readonly ILogger<DeleteTaskCommandHandler> _logger;
        readonly IUnitOfWork _unitOfWork;

        public DeleteTaskCommandHandler(INotificationService notificationService,
            IIdentityService identityService,
            ILogger<DeleteTaskCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _identityService = identityService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            Core.TaskAggregate.Task task = default(Core.TaskAggregate.Task);

            try
            {
                task = await _unitOfWork.Tasks.GetTaskAsync(request.TaskId);
                await _unitOfWork.Tasks.Remove(task);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"---> {_identityService.GetCurrentUserName()} successfully deleted the task" +
                    $"with ID: \"{task.Id}\"");

                await _notificationService.Notify(
                    _identityService.GetCurrentUserIdentity(),
                    true,
                    $"The task was removed.", cancellationToken);

                return Result<bool>.Success().With("The task was removed.");
            }
            catch (Exception ex)
            {

                _logger.LogError($"---> An error occur when {_identityService.GetCurrentUserName()} tried to delete" +
                    $"the task with ID:\"{request.TaskId}\"");

                await _notificationService.Notify(
                    _identityService.GetCurrentUserIdentity(),
                    true,
                    ex.Message, cancellationToken);

                return Result<bool>.Invalid().With(ex.Message);
            }
        }
    }
}
