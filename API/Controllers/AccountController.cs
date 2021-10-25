using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Data.Identity;
using TaskBase.Data.Storage;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        readonly IImageStorage _imageStorage;
        private IWebHostEnvironment _environment;

        public AccountController(
            UserManager<User> userManager,
            IConfiguration configuration,
            IImageStorage imageStorage,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _configuration = configuration;
            _imageStorage = imageStorage;
            _environment = environment;
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginModel.UserName);

                if (user != null)
                {
                    var validPassowrd = await _userManager.CheckPasswordAsync(user, loginModel.Password);
                    var userProfile = await GenerateJwtTokenForUser(user);

                    if(validPassowrd)
                        return Ok(userProfile);
                    else
                        ModelState.AddModelError(string.Empty, "Invalid UserName or Password.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid UserName or Password.");
                }
            }

            return BadRequest(ModelState.Values.Select(x => x.Errors).First().First().ErrorMessage);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if(ModelState.IsValid)
            {
               var result = await _userManager.CreateAsync(new User()
                        {
                            UserName = registerModel.UserName,
                            Email = registerModel.Email
                            
                        }, registerModel.Password);

                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(String.Empty, error.Description);

                    return BadRequest(ModelState.Values.SelectMany(x => x.Errors));
                }
            }

            return BadRequest(ModelState.Values.SelectMany(x => x.Errors));
        }

        [Authorize]
        [HttpPost("ChangeAvatar")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file is null)
                return BadRequest();

            var url = string.Empty;

            using var stream = file.OpenReadStream();
            string fileExtension = Path.GetExtension(file.FileName);

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserData = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);

            var baseDirectory = _environment.ContentRootPath;

            if (string.IsNullOrWhiteSpace(currentUserData.AvatarUrl))
                url = await _imageStorage.UploadImage(stream, fileExtension, baseDirectory);
            else
                url = await _imageStorage.UpdateImage(currentUserData.AvatarUrl.Split("\\")[1], stream, fileExtension, baseDirectory);

            currentUserData.AvatarUrl = url;
            await _userManager.UpdateAsync(currentUserData);

            return Created(url, null);
        }

        private async Task<UserProfileModel> GenerateJwtTokenForUser(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]),
                        new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                        new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())
                    };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            JwtHeader jwtHeader = new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                        SecurityAlgorithms.HmacSha256
                        )
                );

            var jwtToken = new JwtSecurityToken(jwtHeader, new JwtPayload(claims));

            return new UserProfileModel(
                user.Id,
                user.UserName,
                user.Email,
                new JwtSecurityTokenHandler().WriteToken(jwtToken));
        }
    }
}
