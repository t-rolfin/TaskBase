using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskBase.Application.Services;

namespace API.Services
{
    public class IdentityService : IIdentityService
    {
        readonly IHttpContextAccessor _accessor;

        public IdentityService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Guid GetCurrentUserIdentity()
        {
            Guid.TryParse(_accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value, out Guid userId);
            return userId;
        }

        public string GetCurrentUserName()
        {
            return _accessor.HttpContext.User.Identity.Name;
        }
    }
}
