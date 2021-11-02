using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TaskBase.Components.Utils
{
    public static class FailApiResponseExtension
    {
        public static void ConvertToModelStateErrors(this FailApiResponse response, ModelStateDictionary modelState)
        {
            foreach (var error in response.Errors)
            {
                modelState.AddModelError(String.Empty, error);
            }
        }
    }
}
