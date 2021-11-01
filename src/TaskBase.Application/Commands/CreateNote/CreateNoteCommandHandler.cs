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

namespace TaskBase.Application.Commands.CreateNote
{
    public class CreateNoteCommandHandler :
        IRequestHandler<CreateNoteCommand, Result<Note>>
    {
        readonly INotificationService _notificationService;
        readonly IIdentityProvider _identityService;
        readonly ILogger<CreateNoteCommandHandler> _logger;
        readonly IUnitOfWork _unitOfWork;

        public CreateNoteCommandHandler(INotificationService notificationService,
            IIdentityProvider identityService,
            ILogger<CreateNoteCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _identityService = identityService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Note>> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.TaskId, out Guid taskId))
                throw new InvalidCastException("The task id is not in the right format.");

            var task = await _unitOfWork.Tasks.GetTaskAsync(taskId);
            var note = task.CreateNote(request.Content);

            await _unitOfWork.Tasks.UpdateAsync(task, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($" ---> A new note was created for the task: {task.Id}.");

            return Result<Note>.Success(note)
                .With($"A note was added to the task { task.Title.Take(10).ToString().PadRight(3, '.') }");
        }
    }
}
