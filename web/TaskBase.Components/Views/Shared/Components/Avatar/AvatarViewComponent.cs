using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskBase.Components.Services;
using TaskBase.Data.Identity;

namespace TaskBase.Components.Views.Shared.Components.Avatar
{
    public class AvatarViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;
        private readonly IIdentityProvider _identityProvider;

        public AvatarViewComponent(UserManager<User> userManager, IIdentityProvider identityProvider)
        {
            _userManager = userManager;
            _identityProvider = identityProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _identityProvider.GetCurrentUserIdentity();
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

            return View(new AvatarModel() { Value = user.AvatarUrl });
        }

    }
    public class AvatarModel
    {
        public string Value { get; set; }
    }
}
