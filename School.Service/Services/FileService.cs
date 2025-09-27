using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using School.Service.Abstracts;

namespace School.Service.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly long _maxFileSize = 2 * 1024 * 1024; // 2 MB
        private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".pdf", ".xlsx", ".docx"];

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadImage(string folderName, IFormFile file)
        {
            #region Validations

            if (file is null || file.Length == 0)
                throw new ArgumentException("File is empty or null.");

            if (file.Length > _maxFileSize)
                throw new ArgumentException("File size exceeds the maximum allowed size (2MB).");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid file type. Allowed types are: jpg, jpeg, png, gif.");

            #endregion

            var uploadPath = Path.Combine(_env.WebRootPath, folderName); // => ~/wwwroot/images/instructors

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadPath, fileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var relativePath = Path.Combine(folderName, fileName).Replace("\\", "/");
                return $"/{relativePath}";
            }
            catch (Exception ex)
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                throw new IOException("An error occurred while saving the file.", ex);
            }
        }

    }
}
