using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskBase.Application.Services;

namespace TaskBase.Components.Services
{
    public class IdentityProvider : IIdentityService
    {
        readonly IHttpContextAccessor _context;

        public IdentityProvider(IHttpContextAccessor context)
        {
            _context = context;
        }

        public Guid GetCurrentUserIdentity()
        {
            var userId = _context.HttpContext.User.Claims.First(x => x.Type.Contains("nameidentifier")).Value;

            bool isSuccess = Guid.TryParse(userId, out Guid userIdAsGuid);

            if (!isSuccess)
                throw new InvalidCastException();

            return userIdAsGuid;
        }

        public string GetCurrentUserName()
        {
            return _context.HttpContext.User.Identity.Name;
        }
    }
}
