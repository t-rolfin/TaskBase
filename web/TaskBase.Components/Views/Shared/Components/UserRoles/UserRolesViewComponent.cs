using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Components.Services;

namespace TaskBase.Components.Views.Shared.Components.RoleList
{
    public class UserRolesViewComponent : ViewComponent
    {
        private readonly IAuthService _authService;

        public UserRolesViewComponent(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IViewComponentResult> InvokeAsync(UserModel model)
        {
            var availableRoles = EliminateCommonRoles(
                model.UserRoles, await _authService.GetAvailableRolesAsync());

            await Task.CompletedTask;

            return View((model, availableRoles));
        }

        List<string> EliminateCommonRoles(List<string> userRoles, List<string> availableRoles)
        {
            if(userRoles is not null)
            {
                foreach (var userRole in userRoles)
                    availableRoles?.Remove(userRole);

                return availableRoles;
            }

            return new List<string>();
        }
    }
}
