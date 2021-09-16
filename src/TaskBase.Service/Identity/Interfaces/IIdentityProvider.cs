using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Service.Interfaces.Identity
{
    public interface IIdentityProvider
    {
        Task Register(string userName, string password, string emailAddress);
        Task LogIn(string userName, string password);
        Task LogOut();
    }
}
