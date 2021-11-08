using System.IO;
using System.Threading.Tasks;

namespace TaskBase.Data.Storage
{
    public interface IStorage
    {
        Task<string> UpdateImage(string oldImageName, Stream stream, string imgExtension);
        Task<string> UploadImage(Stream stream, string imgExtension);
    }
}