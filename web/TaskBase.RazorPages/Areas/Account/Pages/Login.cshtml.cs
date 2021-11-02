using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TaskBase.Components.Models;
using TaskBase.Components.Services;
using TaskBase.Components.Utils;

namespace TaskBase.RazorPages.Areas.Account
{
    [AllowAnonymous]
    public class LogInModel : PageModel
    {
        private readonly ILogger<LogInModel> _logger;
        private readonly IAuthService _authService;

        public LogInModel(ILogger<LogInModel> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "User Name")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            ReturnUrl = returnUrl;

            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= "/tasks";

            if (!ModelState.IsValid)
                return Page();

            var result = await _authService.Login(new Components.Models.LogInModel 
                { UserName = Input.UserName, Password = Input.Password });
                
            if(result.IsT1)
            {
                result.AsT1.ConvertToModelStateErrors(ModelState);
                return Page();
            }

            return RedirectPermanent(returnUrl);
        }
    }
}
