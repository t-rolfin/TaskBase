using MediatR;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rolfin.Result;
using TaskBase.Application.Models;
using System.Collections.Generic;
using System.Threading;
using TaskBase.Application.Services;

namespace TaskBase.Application.Queries.GetTasksByUser
{
    public class GetTasksByUserQueryHandler 
        : IRequestHandler<GetTasksByUserQuery, Result<IEnumerable<TaskModel>>>
    {
        readonly IQueryRepository _queryRepository;

        public GetTasksByUserQueryHandler(IQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<Result<IEnumerable<TaskModel>>> Handle(GetTasksByUserQuery request, CancellationToken cancellationToken)
        {
            var userTasks = await _queryRepository.GetTasksByUserIdAsync(request.UserId);

            return Enumerable.Count(userTasks) == 0
                ? Result<IEnumerable<TaskModel>>.Invalid()
                : Result<IEnumerable<TaskModel>>.Success(userTasks);
        }
    }
}
