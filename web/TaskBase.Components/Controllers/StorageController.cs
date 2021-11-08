using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TaskBase.Components.Services;
using TaskBase.Components.Views.Shared.Components.Avatar;

namespace TaskBase.Components.Controllers
{
    [Route("[controller]/[action]")]
    public class StorageController : Controller
    {
        private readonly IAuthService _authService;

        public StorageController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file is null)
                return ViewComponent("Avatar", new AvatarModel() { Url = null });

            var url = string.Empty;

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            string fileExtension = Path.GetExtension(file.FileName);

            var result = await _authService.ChangeAvatar(memoryStream.ToArray());
            url = result.Url;

            return ViewComponent("Avatar", new AvatarModel() { Url = url });
        }
    }
}
