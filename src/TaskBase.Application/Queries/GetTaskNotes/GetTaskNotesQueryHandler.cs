using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rolfin.Result;
using TaskBase.Application.Models;
using TaskBase.Application.Services;

namespace TaskBase.Application.Queries.GetTaskNotes
{
    public class GetTaskNotesQueryHandler : IRequestHandler<GetTaskNotesQuery, Result<IEnumerable<NoteModel>>>
    {
        private IQueryRepository _queryRepository;

        public GetTaskNotesQueryHandler(IQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<Result<IEnumerable<NoteModel>>> Handle(GetTaskNotesQuery request, CancellationToken cancellationToken)
        {
            var taskNotes = await _queryRepository
                .GetTaskNotesAsync(request.TaskId.ToString(), cancellationToken);

            return Enumerable.Count(taskNotes) == 0
                ? Result<IEnumerable<NoteModel>>.Invalid()
                : Result<IEnumerable<NoteModel>>.Success(taskNotes);
        }
    }
}
