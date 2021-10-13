using MediatR;
using Rolfin.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Core.Interfaces;
using TaskBase.Core.TaskAggregate;

namespace TaskBase.Application.Commands.CreateNote
{
    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, Result<Note>>
    {
        readonly IUnitOfWork _unitOfWork;

        public CreateNoteCommandHandler(IUnitOfWork unitOfWork)
        {
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

                return Result<Note>.Success(note)
                    .With($"A note was added to the task { task.Title.Take(10).ToString().PadRight(3, '.') }");
            }
            catch (Exception ex)
            {
                return Result<Note>.Invalid().With(ex.Message);
            }
        }
    }
}
