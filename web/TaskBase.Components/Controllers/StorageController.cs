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
        //[HttpPost]
        //public async Task<IActionResult> UploadImage(IFormFile file)  
        //{
        //    if (file is null)
        //        return ViewComponent("Avatar", new AvatarModel() { Value = null });

        //    var url = string.Empty;

        //    using var stream = file.OpenReadStream();
        //    string fileExtension = Path.GetExtension(file.FileName);

        //    var currentUserId = _identityProvider.GetCurrentUserIdentity();
        //    var currentUserData = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == currentUserId.ToString());

        //    if (string.IsNullOrWhiteSpace(currentUserData.AvatarUrl))
        //        url = await _imageStorage.UploadImage(stream, fileExtension);
        //    else
        //        url = await _imageStorage.UpdateImage(currentUserData.AvatarUrl.Split("\\")[1], stream, fileExtension);

        //    currentUserData.AvatarUrl = url;
        //    await _userManager.UpdateAsync(currentUserData);

        //    return ViewComponent("Avatar", new AvatarModel() { Value = url });
        //}
    }
}
