using MediatR;
using Rolfin.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Core.Interfaces;

namespace TaskBase.Application.Commands.EliminateNote
{
    public class EliminateNoteCommandHandler : IRequestHandler<EliminateNoteCommand, Result<bool>>
    {
        readonly IUnitOfWork _unitOfWork;

        public EliminateNoteCommandHandler(IUnitOfWork unitOfWork)
        {
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

                return Result<bool>.Success().With($"The specified note was deleted!");
            }
            catch (Exception ex)
            {
                return Result<bool>.Invalid().With(ex.Message);
            }
        }
    }
}
