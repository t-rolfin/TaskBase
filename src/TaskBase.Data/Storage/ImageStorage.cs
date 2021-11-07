using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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

        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public ImageStorage(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }

        public async Task<string> UpdateImage(string oldImageName, Stream stream, string imgExtension)
        {
            var fullFilePath = Path.Combine(_environment.ContentRootPath, _configuration["Avatar:FolderName"], oldImageName);
            var response = string.Empty;

            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
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
            if(allowExtensions.Any(x => x == imgExtension) && stream != null)
            {
                string fileName = Guid.NewGuid().ToString() + imgExtension;
                string fullFilePath = Path.Combine(_environment.ContentRootPath, _configuration["Avatar:FolderName"], fileName);
                using var fileStream = new FileStream(fullFilePath, FileMode.Create);

                await stream.CopyToAsync(fileStream);

                return fileName;
            }

            return null;
        }
    }
}
