using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Data.Storage
{
    public class ImageStorage : IImageStorage
    {
        private readonly string UPLOAD_PATH = "Avatars";
        private IHostingEnvironment _environment;

        public ImageStorage(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public Task<string> UpdateImage(string oldImageName, Stream stream, string imgExtension)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadImage(Stream stream, string imgExtension)
        {
            string fileName = null;

            if (stream != null)
            {
                fileName = Guid.NewGuid().ToString() + imgExtension;
                string filePath = Path.Combine(_environment.WebRootPath ,UPLOAD_PATH, fileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);

                await stream.CopyToAsync(fileStream);

                return Path.Combine("/", UPLOAD_PATH, fileName);
            }
            else
                return null;
        }
    }
}
