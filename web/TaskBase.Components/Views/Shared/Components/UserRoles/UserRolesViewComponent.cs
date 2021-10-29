using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;
namespace TaskBase.Components.Views.Shared.Components.RoleList
{
    public class UserRolesViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync((string userId, List<string> availableRoles) model)
        {
            //var user = _userManager.Users.FirstOrDefault(x => x.Id == model.userId);
            //var userRoles = await _userManager.GetRolesAsync(user);
            //return View((Guid.Parse(user.Id), userRoles, model.availableRoles));

            return await Task.FromResult(View());
        }
    }
}
