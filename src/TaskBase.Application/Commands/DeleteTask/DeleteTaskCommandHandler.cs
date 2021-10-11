using MediatR;
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
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Result<bool>>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly INotificationService _notificationService;
        readonly IIdentityService _identityService;

        public DeleteTaskCommandHandler(IUnitOfWork unitOfWork,
            INotificationService notificationService,
            IIdentityService identityService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _identityService = identityService;
        }

        public async Task<Result<bool>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            Core.TaskAggregate.Task task = default(Core.TaskAggregate.Task);

            try
            {
                task = await _unitOfWork.Tasks.GetTaskAsync(request.TaskId);
                await _unitOfWork.Tasks.Remove(task);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _notificationService.Notify(
                    _identityService.GetCurrentUserIdentity(),
                    true,
                    $"The task was removed.");

                return Result<bool>.Success().With("The task was removed");
            }
            catch (Exception ex)
            {
                await _notificationService.Notify(
                    _identityService.GetCurrentUserIdentity(),
                    true,
                    ex.Message);

                return Result<bool>.Invalid().With(ex.Message);
            }
        }
    }
}
