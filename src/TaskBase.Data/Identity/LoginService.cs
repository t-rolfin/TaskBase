using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Application.Models;
using TaskBase.Application.Services;
using TaskBase.Data.Exceptions;

namespace TaskBase.Data.Identity
{
    public class LoginService : IIdentityService<User>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IdentityContext _identityContext;
        public LoginService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IdentityContext identityContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _identityContext = identityContext;
        }


        public async Task<bool> CreateAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded == false)
                throw new DuplicateUserNameException(result.Errors.First().Description);

            return result.Succeeded == true ? true : false;
        }

        public async Task<User> FindUserByIdAsync(string userId)
        {
            var user = await _userManager.Users.Where(x => x.Id.Equals(userId))
               .FirstOrDefaultAsync();

            if (user is null)
                throw new InvalidUserNameException(
                    "The provided User Name is invalid, please try again.");

            return user;
        }

        public async Task<User> FindUserByNameAsync(string userName)
        {
            var user = await _userManager.Users.Where(x => x.UserName.Equals(userName))
                .FirstOrDefaultAsync();

            if (user is null)
                throw new InvalidUserNameException(
                    "The provided User Name is invalid, please try again.");

            return user;
        }

        public async Task<IEnumerable<string>> GetRolesAsync(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles;
        }

        public async Task SignInAsync(User user, bool isPersistent)
        {
            await _signInManager.SignInAsync(user, isPersistent);
        }

        public async Task SignInAsync(User user, AuthenticationProperties properties, string authenticationMethod)
        {
            await _signInManager.SignInAsync(user, properties, authenticationMethod);
        }

        public async Task<bool> UpdateAsync(User user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ValidateCredentialsAsync(string userName, string password)
        {
            var user = await FindUserByNameAsync(userName);
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            return result.Succeeded == true ? true : false;
        }

        public async Task<IEnumerable<UserModel>> GetMembersAsync()
        {
            var members = await _userManager.Users
                .Select(x => new UserModel(Guid.Parse(x.Id), x.UserName, x.AvatarUrl))
                .ToListAsync();

            foreach (var member in members)
            {
                var user = await FindUserByIdAsync(member.Id.ToString());
                var userRoles = await GetRolesAsync(user);
                member.UserRoles = userRoles.ToList();
            }

            return members;
        }

        public async Task<IEnumerable<string>> GetAvailableRoles()
        {
            return await _roleManager.Roles.Select(x => x.Name).ToListAsync();
        }

        public async Task<UserModel> AssignRoleToUserAsync(string roleName, string userId)
        {
            var user = await _userManager.Users
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            await _userManager.AddToRoleAsync(user, roleName);
            return await GetUserWithRolesAsync(user);
        }

        public async Task<UserModel> RemoveUserFromRoleAsync(string roleName, string userId)
        {
            var user = await _userManager.Users
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            if(roleName.Equals("Admin"))
                await CheckIfIsLastAdmin();

            await _userManager.RemoveFromRoleAsync(user, roleName);
            return await GetUserWithRolesAsync(user);
        }


        async Task<UserModel> GetUserWithRolesAsync(User user)
        {
            var userModel = new UserModel(Guid.Parse(user.Id), user.UserName, user.AvatarUrl);
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
                userModel.UserRoles.Add(role);

            return userModel;
        }
        async Task CheckIfIsLastAdmin()
        {
            var adminRole = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Name == "Admin");
            var numberOfAdmins = _identityContext.UserRoles
                .Count(x => x.RoleId == adminRole.Id);

            if (numberOfAdmins == 1)
                throw new LastAdminException(
                    "The 'Admin' role cannot be removed for this user.");
        }
    }
}
