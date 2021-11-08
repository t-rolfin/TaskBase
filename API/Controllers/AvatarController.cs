using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskBase.Data.Identity;
using TaskBase.Data.Storage;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class AvatarController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        private readonly IIdentityService<User> _loginService;
        private readonly IStorage _imageStorage;

        public AvatarController(
            IWebHostEnvironment env,
            IIdentityService<User> loginService,
            IStorage imageStorage)
        {
            _env = env;
            _loginService = loginService;
            _imageStorage = imageStorage;
        }

        [HttpGet("{imgName}")]
        public async Task<IActionResult> DisplayImage(string imgName)
        {
            var path = Path.Combine(_env.ContentRootPath, "Avatars", imgName);

            return await Task.FromResult(
                new FileStreamResult(new FileStream(path, FileMode.Open), "image/png"));
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UploadAvatar(IFormFile avatar)
        {
            if (avatar is null)
                return BadRequest();

            User currentUserData = await GetCurrentUser();
            var url = await UploadUserAvatar(avatar, currentUserData.AvatarUrl);
            currentUserData.AvatarUrl = url;
            await _loginService.UpdateAsync(currentUserData);

            return Created("", new { url = $"https://localhost:5001{url}" });
        }

        [Authorize]
        [HttpGet("{userId:Guid}")]
        public async Task<IActionResult> GetAvatarUrl(Guid userId)
        {
            var user = await _loginService.FindUserByIdAsync(userId.ToString());
            return Ok(new { url = $"https://localhost:5001{user.AvatarUrl}" });
        }


        async Task<User> GetCurrentUser()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _loginService.FindUserByIdAsync(currentUserId);
        }
        async Task<string> UploadUserAvatar(IFormFile file, string currentUserAvatar)
        {
            var imgName = string.Empty;

            using var stream = file.OpenReadStream();
            string fileExtension = Path.GetExtension(file.FileName);

            if (string.IsNullOrWhiteSpace(currentUserAvatar))
                imgName = await _imageStorage.UploadImage(stream, fileExtension);
            else
                imgName = await _imageStorage.UpdateImage(currentUserAvatar.Split("\\")[1], stream, fileExtension);

            return $"/api/Avatar/{ imgName }";
        }
    }
}
