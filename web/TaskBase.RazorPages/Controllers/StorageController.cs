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

namespace TaskBase.RazorPages.Controllers
{
    [Route("[controller]/[action]")]
    public class StorageController : Controller
    {
        readonly IImageStorage _imageStorage;
        readonly UserManager<User> _userManager;
        readonly IIdentityProvider _identityProvider;

        public StorageController(IImageStorage imageStorage, 
            UserManager<User> userManager, 
            IIdentityProvider identityProvider)
        {
            _imageStorage = imageStorage;
            _userManager = userManager;
            _identityProvider = identityProvider;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)  
        {
            if (file is null)
                return ViewComponent("Avatar", new AvatarModel() { Value = null });

            using var stream = file.OpenReadStream();
            string fileExtension = Path.GetExtension(file.FileName);
            var url = await _imageStorage.UploadImage(stream, fileExtension);

            var currentUserId = _identityProvider.GetCurrentUserIdentity();
            var currentUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);
            currentUser.AvatarUrl = url;
            await _userManager.UpdateAsync(currentUser);

            return ViewComponent("Avatar", new AvatarModel() { Value = url });
        }
    }
}
