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
using TaskBase.Components.Services;

namespace TaskBase.RazorPages.Areas.Account.Pages
{
    [Authorize(Roles = "Admin")]
    public class UsersModel : PageModel
    {
        private readonly IAuthService _authService;

        public UsersModel(IAuthService authService)
        {
            _authService = authService;
        }

        List<UserModel> _users = new List<UserModel>();

        public List<UserModel> Users
        {
            get { return _users; }
            protected set { _users = value; }
        }
        

        public async Task OnGetAsync() 
        { 
            _users = await _authService.GetMembersAsync(); 
        }

        public async Task<IActionResult> OnPostRemoveFromRole(Guid userId, string role)
        {
            var response = await _authService.FireUserToRole(role, userId.ToString());
            return ViewComponent("UserRoles", response);
        }

        public async Task<IActionResult> OnPostAssignToRole(Guid userId, string role)
        {
            var response = await _authService.AssignUserToRole(role, userId.ToString());
            return ViewComponent("UserRoles", response);
        }

    }
}