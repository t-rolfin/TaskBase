using System;
using System.Threading.Tasks;

namespace TaskBase.Data.Identity
{
    public interface IAuthTokenFactory
    {
        Task<string> GetTokenAsync(string userId);
        Task<string> GetTokenByUserNameAsync(string userName);
    }
}