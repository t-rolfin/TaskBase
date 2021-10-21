﻿using MediatR;
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

namespace TaskBase.Application.Commands.EliminateNote
{
    public class EliminateNoteCommandHandler : IRequestHandler<EliminateNoteCommand, Result<bool>>
    {
        readonly INotificationService _notificationService;
        readonly IIdentityService _identityService;
        readonly ILogger<EliminateNoteCommandHandler> _logger;
        readonly IUnitOfWork _unitOfWork;

        public EliminateNoteCommandHandler(INotificationService notificationService,
            IIdentityService identityService,
            ILogger<EliminateNoteCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _identityService = identityService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(EliminateNoteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetTaskWithNotesAsync(request.TaskId);
                task.EliminateNote(request.NoteId);

                await _unitOfWork.Tasks.UpdateAsync(task, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"---> The note with ID: {request.NoteId} was eliminated.");

                return Result<bool>.Success().With($"The specified note was deleted!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"---> The note with ID: \"{request.NoteId}\" couldn't be deleted, error message:" +
                    $"\"{ex.Message}\"");

                return Result<bool>.Invalid().With(ex.Message);
            }
        }
    }
}
