using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TaskBase.Components.Models;
using TaskBase.Data.Identity;

namespace TaskBase.Components.Areas.Identity.Pages.Account
{
    public class UsersModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UsersModel> _log;

        [BindProperty]
        public IEnumerable<User> _users { get; set; }

        [BindProperty]
        public List<string> _availableRoles { get; set; }

        public UsersModel(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<UsersModel> log)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _log = log;
        }

        public void OnGet() { updateDate(); }

        public async Task<IActionResult> OnPostRemoveFromRole(Guid userId, string role)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId.ToString());
            var result = await _userManager.RemoveFromRoleAsync(user, role);
            
            return ViewComponent("RoleList", user);
        }

        public async Task<IActionResult> OnPostAssignToRole(Guid userId, string role)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId.ToString());
            var result = await _userManager.AddToRoleAsync(user, role);
            _log.LogInformation(result.Succeeded.ToString());

            return ViewComponent("RoleList", user);
        }

        private void updateDate()
        {
            _users = _userManager.Users.ToList();
            _availableRoles = _roleManager.Roles.Select(x => x.Name).ToList();
        }

    }
}
