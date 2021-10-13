using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskBase.Application.Services;
using TaskBase.Components.Services;
using TaskBase.Data.Identity;

namespace TaskBase.Components.Views.Shared.Components.Avatar
{
    public class AvatarViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;
        private readonly IIdentityService _identityProvider;

        public AvatarViewComponent(UserManager<User> userManager, IIdentityService identityProvider)
        {
            _userManager = userManager;
            _identityProvider = identityProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _identityProvider.GetCurrentUserIdentity();
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId.ToString());

            return View(new AvatarModel() { Value = user.AvatarUrl });
        }

    }
    public class AvatarModel
    {
        public string Value { get; set; }
    }
}
