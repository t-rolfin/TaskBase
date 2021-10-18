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
using TaskBase.Core.NotificationAggregate;
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
            User user = default;
            Notification notification = default;

            try
            {
                bool isPersistentNotification = false;

                if (string.IsNullOrWhiteSpace(request.AssignTo))
                    user = await _unitOfWork.Tasks.GetUserByIdAsync(_identityService.GetCurrentUserIdentity());
                else
                {
                    user = await _unitOfWork.Tasks.GetUserByUserNameAsync(request.AssignTo);
                    isPersistentNotification = true;
                }

                var priorityLevel = await _unitOfWork.Tasks.GetPriorityLevelAsync(request.PriorityLevel);

                var task = new CoreTask(request.Title, 
                    request.Description, request.DueDate, 
                    priorityLevel,
                    user);

                await _unitOfWork.Tasks.AddAsync(task, cancellationToken);

                if(isPersistentNotification)
                    notification = await _unitOfWork.Notifications.AddAsync(
                        new Notification(
                            task.Title,
                            "A new task was assign to you, use refresh button from 'To Do' container to see the task", 
                            user.Id), cancellationToken
                        );

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await SendNotifications(user, notification?.Id.ToString(), isPersistentNotification, task, cancellationToken);

                return Result.Success(task).With("A new task was successfully created.");
            }
            catch (Exception ex)
            {
                await _notificationService.Notify(_identityService.GetCurrentUserIdentity(), false, ex.Message, cancellationToken);
                return Result<CoreTask>.Invalid().With(ex.Message);
            }
        }

        async System.Threading.Tasks.Task SendNotifications(User user, string notificationId, bool isPersistentNotification, CoreTask task, CancellationToken cancellationToken)
        {
            if (isPersistentNotification)
            {
                await _notificationService.PersistentNotification(
                    Guid.Parse(notificationId),
                    user.Id,
                    true,
                    "A new task was assign to you, use refresh button from 'To Do' container to see the task",
                    task.Title,
                    cancellationToken
                    );

                await _notificationService.Notify(
                    _identityService.GetCurrentUserIdentity(),
                    true,
                    $"The task was successfully assigned to {user.FullName}.",
                    cancellationToken
                    );

            }
            else
            {
                await _notificationService.Notify(
                    user.Id,
                    true,
                    "A new task was created.",
                    cancellationToken
                    );
            }
        }
    }
}
