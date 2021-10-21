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

namespace TaskBase.Application.Commands.EditNote
{
    public class EditNoteCommandHandler :
        IRequestHandler<EditNoteCommand, Result<bool>>
    {
        readonly INotificationService _notificationService;
        readonly IIdentityService _identityService;
        readonly ILogger<EditNoteCommandHandler> _logger;
        readonly IUnitOfWork _unitOfWork;

        public EditNoteCommandHandler(INotificationService notificationService,
            IIdentityService identityService,
            ILogger<EditNoteCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _identityService = identityService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(EditNoteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetTaskWithNotesAsync(request.TaskId);
                task.EditeNote(request.NoteId, request.Content);

                await _unitOfWork.Tasks.UpdateAsync(task, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"---> The note with ID: \"{request.NoteId}\" was successfully edited.");

                return Result<bool>.Success().With($"The note was successfully updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"---> The note with ID: \"{request.NoteId}\" could not be edited, error message: " +
                    $"{ex.Message}");

                return Result<bool>.Invalid().With(ex.Message);
            }
        }
    }
}
