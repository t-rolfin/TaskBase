using MediatR;
using Rolfin.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Core.Interfaces;

namespace TaskBase.Application.Commands.EditNote
{
    public class EditNoteCommandHandler : IRequestHandler<EditNoteCommand, Result<bool>>
    {
        readonly IUnitOfWork _unitOfWork;

        public EditNoteCommandHandler(IUnitOfWork unitOfWork)
        {
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

                return Result<bool>.Success().With($"The note was successfully updated.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Invalid().With(ex.Message);
            }
        }
    }
}
