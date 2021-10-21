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
        readonly IIdentityService _identityService;
        readonly ILogger<CreateNoteCommandHandler> _logger;
        readonly IUnitOfWork _unitOfWork;

        public CreateNoteCommandHandler(INotificationService notificationService,
            IIdentityService identityService,
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
            try
            {
                var task = await _unitOfWork.Tasks.GetTaskAsync(request.TaskId);
                var note = task.CreateNote(request.Content);

                await _unitOfWork.Tasks.UpdateAsync(task, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($" ---> A new note was created for the task: {task.Id}.");

                return Result<Note>.Success(note)
                    .With($"A note was added to the task { task.Title.Take(10).ToString().PadRight(3, '.') }");
            }
            catch (Exception ex)
            {
                _logger.LogError($" ---> A error occur at note creation, error message: \"{ex.Message}\"");

                return Result<Note>.Invalid().With(ex.Message);
            }
        }
    }
}
