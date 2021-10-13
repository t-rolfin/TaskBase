using MediatR;
using Rolfin.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Core.Interfaces;

namespace TaskBase.Application.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Result<bool>>
    {
        readonly IUnitOfWork _unitOfWork;

        public UpdateTaskCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetTaskAsync(request.TaskId);

                if (!string.IsNullOrWhiteSpace(request.Description)) task.EditDescription(request.Description);

                if (!string.IsNullOrWhiteSpace(request.Title)) task.EditTitle(request.Title);

                await _unitOfWork.Tasks.UpdateAsync(task, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success().With("Description for the task was changed!");
            }
            catch (Exception ex)
            {
                return Result<bool>.Invalid().With(ex.Message);
            }
        }
    }
}
