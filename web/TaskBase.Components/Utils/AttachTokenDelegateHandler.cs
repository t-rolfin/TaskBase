using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskBase.Components.Utils
{
    public class AttachTokenDelegateHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AttachTokenDelegateHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _contextAccessor.HttpContext.Session.GetString("access_token");

            if(token != null)
                request.Headers.Authorization = new AuthenticationHeaderValue(
                        "Bearer", 
                        token
                    );

            return base.SendAsync(request, cancellationToken);
        }
    }
}
