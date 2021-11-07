using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Application.Commands.CreateUser;
using TaskBase.Application.Models;
using TaskBase.Application.Services;
using TaskBase.Data.Identity;
using TaskBase.Data.Storage;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly ILoginService<User> _loginService;
        private readonly IAuthTokenFactory _tokenFactory;

        public AccountController(
            ILoginService<User> loginService,
            IAuthTokenFactory tokenFactory)
        {
            _loginService = loginService;
            _tokenFactory = tokenFactory;
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(LoginModel loginModel)
        {
            var user = await _loginService.FindUserByNameAsync(loginModel.UserName);

            if (user is not null)
            {
                var isValidCredentials = await _loginService.ValidateCredentialsAsync(
                    loginModel.UserName, loginModel.Password);

                var access_token = await _tokenFactory.GetTokenAsync(user.Id);
                var response = await GenerateByteArrayResponse(isValidCredentials, access_token);

                return string.IsNullOrWhiteSpace(response)
                    ? BadRequest()
                    : Ok(response);
            }

            return BadRequest();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            var user = new User(registerModel.UserName, registerModel.Email, "");
            await Mediator.Send(new CreateUserCommand(user.Id, user.UserName));
            var result = await _loginService.CreateAsync(user, registerModel.Password);
            await _loginService.AssignRoleToUserAsync("Member", user.Id);

            return result == true ? Ok() : BadRequest();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("Users")]
        public async Task<IActionResult> Users()
        {
            return Ok(await _loginService.GetMembersAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Roles")]
        public async Task<IActionResult> Roles()
        {
            return Ok(await _loginService.GetAvailableRoles());
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Roles/Assign")]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleModel model)
        {
            var result = await _loginService.AssignRoleToUserAsync(model.RoleName, model.UserId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Roles/Fire")]
        public async Task<IActionResult> FiresUserFromRole(FireRoleModel model)
        {
            var result = await _loginService.RemoveUserFromRoleAsync(model.RoleName, model.UserId);
            return Ok(result);
        }



        Task<string> GenerateByteArrayResponse(bool isValidCredentials, string access_token)
        {
            if (isValidCredentials == true && !string.IsNullOrWhiteSpace(access_token))
            {
                var responseObject = new
                {
                    access_token,
                    token_type = "Bearer"
                };

                var serializedResponse = JsonConvert.SerializeObject(responseObject);

                return Task.FromResult(serializedResponse);
            }

            return Task.FromResult(string.Empty);
        }
    }
}
