using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaskBase.RazorPages.Areas.Account.Pages
{
    public class AccessDeniedModel : PageModel
    {
        [FromQuery]
        string ReturnUrl { get; set; }

        public void OnGet()
        {
        }
    }
}
