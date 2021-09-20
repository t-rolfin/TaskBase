using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Data.Identity;

namespace TaskBase.Components.Views.Shared.Components.RoleList
{
    public class RoleListViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public RoleListViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return View((Guid.Parse(user.Id), userRoles));
        }
    }
}
