using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TaskBase.Components.Services;
using TaskBase.Components.Views.Shared.Components.Avatar;
using TaskBase.Data.Identity;
using TaskBase.Data.Storage;

namespace TaskBase.Components.Controllers
{
    [Route("[controller]/[action]")]
    public class StorageController : Controller
    {
        readonly IIdentityProvider _identityProvider;
        readonly IImageStorage _imageStorage;
        readonly UserManager<User> _userManager;

        public StorageController(IImageStorage imageStorage, 
            UserManager<User> userManager, 
            IIdentityProvider identityProvider)
        {
            _identityProvider = identityProvider;
            _imageStorage = imageStorage;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)  
        {
            if (file is null)
                return ViewComponent("Avatar", new AvatarModel() { Value = null });

            var url = string.Empty;

            using var stream = file.OpenReadStream();
            string fileExtension = Path.GetExtension(file.FileName);

            var currentUserId = _identityProvider.GetCurrentUserIdentity();
            var currentUserData = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);

            if (string.IsNullOrWhiteSpace(currentUserData.AvatarUrl))
                url = await _imageStorage.UploadImage(stream, fileExtension);
            else
                url = await _imageStorage.UpdateImage(currentUserData.AvatarUrl.Split("\\")[1], stream, fileExtension);

            currentUserData.AvatarUrl = url;
            await _userManager.UpdateAsync(currentUserData);

            return ViewComponent("Avatar", new AvatarModel() { Value = url });
        }
    }
}
