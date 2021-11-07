using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskBase.Components.Services;

namespace TaskBase.Components.Views.Shared.Components.Avatar
{
    public class AvatarViewComponent : ViewComponent
    {
        private readonly IAuthService _authService;

        public AvatarViewComponent(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IViewComponentResult> InvokeAsync(AvatarModel avatar)
        {
            if(string.IsNullOrWhiteSpace(avatar.Url))
                avatar = await _authService.GetAvatarUrlAsync(
                    HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);

            return await Task.FromResult(View(avatar));
        }

    }
    public class AvatarModel
    {
        public string Url { get; set; }
    }
}
