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
using TaskBase.Application.Services;
using TaskBase.Data.Identity;
using TaskBase.Data.Storage;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILoginService<User> _loginService;
        private readonly IImageStorage _imageStorage;
        private readonly IAuthTokenFactory _tokenFactory;
        private IWebHostEnvironment _environment;

        public AccountController(
            ILoginService<User> loginService,
            IImageStorage imageStorage,
            IWebHostEnvironment environment,
            IAuthTokenFactory tokenFactory)
        {
            _loginService = loginService;
            _imageStorage = imageStorage;
            _environment = environment;
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

                var access_token = await _tokenFactory.GetToken(Guid.Parse(user.Id));
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
            var result = await _loginService.CreateAsync(user, registerModel.Password);
            return result == true ? Ok() : BadRequest();
        }

        [Authorize]
        [HttpPost("ChangeAvatar")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file is null)
                return BadRequest();

            User currentUserData = await GetCurrentUser();
            var url = await UploadUserAvatar(file, currentUserData.AvatarUrl);
            currentUserData.AvatarUrl = url;
            await _loginService.UpdateAsync(currentUserData);

            return Created(url, null);
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


        async Task<User> GetCurrentUser()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _loginService.FindUserByIdAsync(currentUserId);
        }
        async Task<string> UploadUserAvatar(IFormFile file, string currentUserAvatar)
        {
            var url = string.Empty;

            using var stream = file.OpenReadStream();
            string fileExtension = Path.GetExtension(file.FileName);
            var baseDirectory = _environment.ContentRootPath;

            if (string.IsNullOrWhiteSpace(currentUserAvatar))
                url = await _imageStorage.UploadImage(stream, fileExtension, baseDirectory);
            else
                url = await _imageStorage.UpdateImage(currentUserAvatar.Split("\\")[1], stream, fileExtension, baseDirectory);

            return url;
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
