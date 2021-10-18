using MediatR;
using Rolfin.Result;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Models;
using TaskBase.Application.Services;

namespace TaskBase.Application.Queries.GetTask
{
    public class GetTaskQueryHandler : IRequestHandler<GetTaskQuery, Result<TaskModel>>
    {

        readonly IQueryRepository _queryRepository;

        public GetTaskQueryHandler(IQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<Result<TaskModel>> Handle(GetTaskQuery request, CancellationToken cancellationToken)
        {
            return await _queryRepository.GetTaskAsync(request.TaskId);
        }
    }
}
