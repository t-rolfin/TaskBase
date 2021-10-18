using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rolfin.Result;
using TaskBase.Core.Interfaces;

namespace TaskBase.Application.Commands.ChangeTaskState
{
    public class ChangeTaskStateCommandHandler : IRequestHandler<ChangeTaskStateCommand, Result<bool>>
    {
        readonly IUnitOfWork _unitOfWork;

        public ChangeTaskStateCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(ChangeTaskStateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetTaskAsync(request.TaskId);

                task.ChangeTaskState(request.TaskState);
                await _unitOfWork.Tasks.UpdateAsync(task, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success(true).With($"The state of task was successfully change in to ${nameof(request.TaskState)}");
            }
            catch (Exception ex)
            {
                return Result<bool>.Invalid().With(ex.Message);
            }
        }
    }
}
