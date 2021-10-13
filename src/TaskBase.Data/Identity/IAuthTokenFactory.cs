using System;
using System.Threading.Tasks;

namespace TaskBase.Data.Identity
{
    public interface IAuthTokenFactory
    {
        Task<string> GetToken(Guid userId);
    }
}