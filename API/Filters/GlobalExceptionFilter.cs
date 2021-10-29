using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var problemDetails = new SimpleProblemDetails()
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status400BadRequest,
                Detail = "Please refer to the errors property for additional details."
            };

            if (context.Exception.GetType() == typeof(ValidationException))
            {
                var validationException = context.Exception as ValidationException;
                foreach (var errors in validationException.Errors)
                {
                    problemDetails.Errors.Add(errors.ErrorMessage);
                }
            }
            else
            {
                problemDetails.Errors.Add(context.Exception.Message);
            }

            context.Result = new BadRequestObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            context.ExceptionHandled = true;

        }
    }
}
