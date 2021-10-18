using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class BaseController: ControllerBase
    {
        IMediator _mediator;
        protected IMediator Mediator
            => _mediator ??= (IMediator)HttpContext.RequestServices.GetService(typeof(IMediator));
    }
}
