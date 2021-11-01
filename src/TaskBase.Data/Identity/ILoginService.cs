﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Data.Identity
{
    public interface ILoginService<T>
    {
        Task<bool> ValidateCredentialsAsync(string userName, string password);
        Task<T> FindUserByNameAsync(string userName);
        Task<T> FindUserByIdAsync(string userId);
        Task SignInAsync(T user, bool isPersistent = false);
        Task SignInAsync(T user, AuthenticationProperties properties, string authenticationMethod);
        Task<bool> CreateAsync(T user, string password);
        Task<bool> UpdateAsync(T user);
        Task<IEnumerable<string>> GetRolesAsync(T user);
    }
}
