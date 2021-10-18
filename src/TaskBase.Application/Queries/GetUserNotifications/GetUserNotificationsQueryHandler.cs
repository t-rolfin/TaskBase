using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rolfin.Result;
using TaskBase.Application.Models;
using TaskBase.Application.Services;

namespace TaskBase.Application.Queries.GetUserNotifications
{
    public class GetUserNotificationsQueryHandler
        : IRequestHandler<GetUserNotificationsQuery, Result<IEnumerable<NotificationModel>>>
    {
        readonly IQueryRepository _queryRepository;

        public GetUserNotificationsQueryHandler(IQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<Result<IEnumerable<NotificationModel>>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var userNotifications = await _queryRepository.GetUserNotificationsAsync(request.UserId, cancellationToken);
            
            return Enumerable.Count(userNotifications) == 0
                ? Result<IEnumerable<NotificationModel>>.Invalid()
                : Result<IEnumerable<NotificationModel>>.Success(userNotifications);
        }
    }
}
