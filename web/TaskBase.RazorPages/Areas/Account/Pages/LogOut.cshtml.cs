using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskBase.Components.Services;

namespace TaskBase.RazorPages.Areas.Account
{
    public class LogOutModel : PageModel
    {
        private readonly IAuthService _authService;

        public LogOutModel(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            await _authService.LogOut();
            return RedirectPermanent("/");
        }
    }
}
