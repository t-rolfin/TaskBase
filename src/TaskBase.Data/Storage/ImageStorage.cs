using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Data.Identity;

namespace TaskBase.Data.Storage
{
    public class ImageStorage : IImageStorage
    {
        string[] allowExtensions = new string[] { ".png", ".jpg" };

        private readonly string UPLOAD_PATH = "Avatars";
        private IWebHostEnvironment _environment;

        public ImageStorage(IWebHostEnvironment environment, UserManager<User> userManager)
        {
            _environment = environment;
        }

        public async Task<string> UpdateImage(string oldImageName, Stream stream, string imgExtension)
        {
            var filePath = Path.Combine(_environment.WebRootPath, UPLOAD_PATH, oldImageName);
            var response = string.Empty;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                response = await UploadImage(stream, imgExtension);
            }
            else
            {
                response = await UploadImage(stream, imgExtension);
            }

            return response;
        }

        public async Task<string> UploadImage(Stream stream, string imgExtension)
        {
            string fileName = null;

            if(allowExtensions.Any(x => x == imgExtension) && stream != null)
            {
                fileName = Guid.NewGuid().ToString() + imgExtension;
                string filePath = Path.Combine(_environment.WebRootPath, UPLOAD_PATH, fileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);

                await stream.CopyToAsync(fileStream);

                return Path.Combine("/", UPLOAD_PATH, fileName);
            }

            return null;
        }
    }
}
