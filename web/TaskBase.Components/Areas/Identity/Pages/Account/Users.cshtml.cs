using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskBase.Components.Models;
using TaskBase.Data.Identity;

namespace TaskBase.Components.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Admin")]
    public class UsersModel : PageModel
    {
        List<UserModel> _users;
        
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UsersModel> _log;

        public List<UserModel> Users { 
            get { return _users; }
            protected set { _users = value; }
        }

        public UsersModel(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<UsersModel> log)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _log = log;
        }

        public async Task OnGetAsync() { await GetData(); }

        public async Task<IActionResult> OnPostRemoveFromRole(Guid userId, string role)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId.ToString());
            await _userManager.RemoveFromRoleAsync(user, role);

            await _userManager.UpdateSecurityStampAsync(user);

            var availableRoles = await GetUserAvailableRoles(user);

            _log.LogInformation($"The member {user.UserName} is not longer a/an {role}.");

            return ViewComponent("UserRoles", (user.Id, availableRoles));
        }

        public async Task<IActionResult> OnPostAssignToRole(Guid userId, string role)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId.ToString());
            var result = await _userManager.AddToRoleAsync(user, role);

            await _userManager.UpdateSecurityStampAsync(user);

            var availableRoles = await GetUserAvailableRoles(user);

            _log.LogInformation($"The member {user.UserName} is now a/an {role}.");

            return ViewComponent("UserRoles", (user.Id, availableRoles));
        }

        async Task GetData()
        {
            if (_users == null)
                _users = new();

            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var userAvailableRoles = await EliminateCommonRoles(userRoles.ToList());

                _users.Add(
                    new UserModel(
                        user.Id,
                        user.UserName,
                        userAvailableRoles
                        )
                    );
            }
        }

        async Task<List<string>> GetUserAvailableRoles(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            return await EliminateCommonRoles(userRoles.ToList());
        }

        async Task<List<string>> EliminateCommonRoles(List<string> userRoles)
        {
            var roles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();

            foreach (var userRole in userRoles)
                roles.Remove(userRole);

            return roles;
        }

    }
}
