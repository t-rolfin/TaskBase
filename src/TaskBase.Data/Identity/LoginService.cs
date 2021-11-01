﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Application.Services;
using TaskBase.Data.Exceptions;

namespace TaskBase.Data.Identity
{
    public class LoginService : ILoginService<User>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<bool> CreateAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
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
    }
}
