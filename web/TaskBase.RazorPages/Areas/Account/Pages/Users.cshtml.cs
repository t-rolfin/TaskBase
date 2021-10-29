using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TaskBase.Components.Models;

namespace TaskBase.RazorPages.Areas.Account.Pages
{
    [Authorize(Roles = "Admin")]
    public class UsersModel : PageModel
    {
        List<UserModel> _users = new List<UserModel>();
        //delegate Task<IdentityResult> RoleAction(User user, string role);

        public List<UserModel> Users
        {
            get { return _users; }
            protected set { _users = value; }
        }
        

        public async Task OnGetAsync() { await Task.CompletedTask; }

        //public async Task<IActionResult> OnPostRemoveFromRole(Guid userId, string role)
        //{
        //    (string, List<string>) response = await ChangeUserRoles(
        //            userId, role,
        //            (User user, string role) =>
        //            {
        //                return _userManager.RemoveFromRoleAsync(user, role);
        //            }
        //        );

        //    return ViewComponent("UserRoles", response);
        //}

        //public async Task<IActionResult> OnPostAssignToRole(Guid userId, string role)
        //{
        //    (string, List<string>) response = await ChangeUserRoles(
        //            userId, role,
        //            (User user, string role) =>
        //            {
        //                return _userManager.AddToRoleAsync(user, role);
        //            }
        //        );

        //    return ViewComponent("UserRoles", response);
        //}


        //// private methods
        //async Task<(string UserId, List<string> AvailableRoles)> ChangeUserRoles(
        //    Guid userId, string role, RoleAction roleAction)
        //{
        //    var user = _userManager.Users.FirstOrDefault(x => x.Id == userId.ToString());
        //    var response = await roleAction.Invoke(user, role);

        //    if (response.Succeeded)
        //        await _userManager.UpdateSecurityStampAsync(user);

        //    var availableRoles = await GetUserAvailableRoles(user);

        //    _log.LogInformation($"{user.UserName} is now a/an {role}.");

        //    return (user.Id, availableRoles);
        //}

        //async Task FetchUsers()
        //{
        //    if (_users == null) _users = new();

        //    var users = await _userManager.Users.ToListAsync();

        //    foreach (var user in users)
        //    {
        //        var userRoles = await _userManager.GetRolesAsync(user);
        //        var userAvailableRoles = await EliminateCommonRoles(userRoles.ToList());

        //        _users.Add(
        //            new UserModel(
        //                user.Id,
        //                user.UserName,
        //                user.AvatarUrl,
        //                userAvailableRoles
        //                )
        //            );
        //    }
        //}

        //async Task<List<string>> GetUserAvailableRoles(User user)
        //{
        //    var userRoles = await _userManager.GetRolesAsync(user);

        //    return await EliminateCommonRoles(userRoles.ToList());
        //}

        //async Task<List<string>> EliminateCommonRoles(List<string> userRoles)
        //{
        //    var roles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();

        //    foreach (var userRole in userRoles)
        //        roles.Remove(userRole);

        //    return roles;
        //}

    }
}