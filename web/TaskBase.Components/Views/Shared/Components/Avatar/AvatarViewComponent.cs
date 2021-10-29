using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskBase.Components.Services;

namespace TaskBase.Components.Views.Shared.Components.Avatar
{
    public class AvatarViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var userId = _identityProvider.GetCurrentUserIdentity();
            //var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId.ToString());

            //return View(new AvatarModel() { Value = user.AvatarUrl });

            return await Task.FromResult(View(new AvatarModel() { Value = "" }));
        }

    }
    public class AvatarModel
    {
        public string Value { get; set; }
    }
}
