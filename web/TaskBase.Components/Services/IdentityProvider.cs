using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskBase.Components.Services
{
    public class IdentityProvider : IIdentityProvider
    {
        readonly IHttpContextAccessor _context;

        public IdentityProvider(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string GetCurrentUserIdentity()
        {
            return _context.HttpContext.User.Claims.First(x => x.Type.Contains("nameidentifier")).Value;
        }

        public string GetCurrentUserName()
        {
            return _context.HttpContext.User.Identity.Name;
        }
    }
}
