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
using TaskBase.Core.TaskAggregate;
using CoreTask = TaskBase.Core.TaskAggregate.Task;

namespace TaskBase.Application.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<CoreTask>>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly INotificationService _notificationService;
        readonly IIdentityService _identityService;

        public CreateTaskCommandHandler(IUnitOfWork unitOfWork, INotificationService notificationService, IIdentityService identityService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _identityService = identityService;
        }

        public async Task<Result<CoreTask>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var user = default(User);

            try
            {
                if (string.IsNullOrWhiteSpace(request.AssignTo))
                    user = await _unitOfWork.Tasks.GetUserByIdAsync(
                        _identityService.GetCurrentUserIdentity()
                        );
                else
                    user = await _unitOfWork.Tasks.GetUserByUserNameAsync(request.AssignTo);

                var task = new CoreTask(request.Title, request.Description, request.DueDate, user);
                await _unitOfWork.Tasks.AddAsync(task, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _notificationService.Notify(user.Id, true, "A new task was successfully created.");

                return Result.Success(task).With("A new task was successfully created.");
            }
            catch (Exception ex)
            {
                await _notificationService.Notify(user.Id, false, "A new task was successfully created.");
                return Result<CoreTask>.Invalid().With(ex.Message);
            }
        }
    }
}
