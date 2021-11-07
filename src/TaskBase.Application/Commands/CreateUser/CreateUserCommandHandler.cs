using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Core.Interfaces;
using TaskBase.Core.TaskAggregate;

namespace TaskBase.Application.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
    {
        public IUnitOfWork _uow;

        public CreateUserCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(Guid.Parse(request.UserId), request.FullName);
            await _uow.Tasks.CreateUser(user);
            await _uow.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
