using MediatR;
using System;

namespace TaskBase.Application.Commands.CreateUser
{
    public record CreateUserCommand(string UserId, string FullName) : IRequest<Unit>;
}
