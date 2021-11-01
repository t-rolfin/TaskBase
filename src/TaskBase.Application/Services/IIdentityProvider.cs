using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Application.Services
{
    public interface IIdentityProvider
    {
        Guid GetCurrentUserIdentity();
        string GetCurrentUserName();
    }
}
