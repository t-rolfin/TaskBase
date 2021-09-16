using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Service.Interfaces.Identity;

namespace TaskBase.Service.Identity
{
    public class IdentityProvider : IIdentityProvider
    {
        readonly UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;

        public IdentityProvider(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Task LogIn(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        public Task Register(string userName, string password, string emailAddress)
        {
            throw new NotImplementedException();
        }
    }
}
