using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskBase.Application.Commands.CreateTask;
using TaskBase.Core.Interfaces;

namespace TaskBase.Application.Behaviors
{
    public class UserExistValidatorBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserExistValidatorBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if(request.GetType() == typeof(CreateTaskCommand) 
                && !string.IsNullOrWhiteSpace((request as CreateTaskCommand).AssignTo))
            {
                var convertedRequest = request as CreateTaskCommand;
                await _unitOfWork.Tasks.GetUserByUserNameAsync(convertedRequest.AssignTo);
            }

            return await next();
        }
    }
}
