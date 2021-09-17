using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskBase.Components.Services
{
    public interface IIdentityProvider
    {
        string GetCurrentUserIdentity();
        string GetCurrentUserName();
    }
}
