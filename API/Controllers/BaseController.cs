using API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rolfin.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class BaseController: ControllerBase
    {
        IMediator _mediator;
        protected IMediator Mediator
            => _mediator ??= (IMediator)HttpContext.RequestServices.GetService(typeof(IMediator));

        protected async Task<IActionResult> SendCreateWithMediator<T>(
            IRequest<Result<T>> model,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await Mediator.Send(model, cancellationToken);

            return result.IsSuccess
                ? Created("", result.Value)
                : BadRequest(result.MetaResult.Message);
        }

        protected async Task<IActionResult> SendWithMediator<T>(
            IRequest<Result<T>> model,
            bool returnValue = false,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await Mediator.Send(model, cancellationToken);

            return result.IsSuccess
                ? Ok(returnValue ? result.Value : result.MetaResult)
                : BadRequest(result.MetaResult.Message);
        }

    }
}
