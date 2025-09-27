using Microsoft.AspNetCore.Http;

namespace School.Service.Abstracts
{
    public interface IFileService
    {
        public Task<string> UploadImage(string location, IFormFile file);
    }
}
