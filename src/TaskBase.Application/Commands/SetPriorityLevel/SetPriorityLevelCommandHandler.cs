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
using TaskBase.Core.TaskAggregate;

namespace TaskBase.Application.Commands.SetLowPriorityLevel
{
    public class SetPriorityLevelCommandHandler : IRequestHandler<SetPriorityLevelCommand, Result<bool>>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly ILogger<SetPriorityLevelCommandHandler> _log;
        readonly INotificationService _notificationService;
        readonly IIdentityService _identityService;

        public SetPriorityLevelCommandHandler(IUnitOfWork unitOfWork,
            ILogger<SetPriorityLevelCommandHandler> log,
            INotificationService notificationService,
            IIdentityService identityService)
        {
            _unitOfWork = unitOfWork;
            _log = log;
            _notificationService = notificationService;
            _identityService = identityService;
        }

        public async Task<Result<bool>> Handle(SetPriorityLevelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetTaskAsync(request.TaskId);
                var priorityLevel = await _unitOfWork.Tasks.GetPriorityLevelAsync(request.PriorityLevelKey);

                task.SetLowPriorityLevel();
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _notificationService.Notify(_identityService.GetCurrentUserIdentity(), true,
                    $"The priority level was successfully changed to { priorityLevel.DisplayName }", cancellationToken);

                return Result<bool>.Success().With($"The priority level was successfully changed to { priorityLevel.DisplayName }");
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);

                await _notificationService.Notify(
                    _identityService.GetCurrentUserIdentity(),false, 
                    "Priority level for this task couldn't be set.", cancellationToken);

                return Result<bool>.Invalid().With("Priority level for this task couldn't be set.");
            }
        }
    }
}
