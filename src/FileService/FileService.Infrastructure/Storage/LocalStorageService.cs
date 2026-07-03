using FileService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace FileService.Infrastructure.Storage
{
    public class LocalStorageService : IStorageService
    {
        private readonly IWebHostEnvironment _environment;

        public LocalStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// Lưu file vào wwwroot/uploads
        /// </summary>
        public async Task<string> SaveAsync(IFormFile file, string fileName)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var uploadFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var fullPath = Path.Combine(uploadFolder, fileName);

            await using var stream = new FileStream(
                fullPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None);

            await file.CopyToAsync(stream);

            return fullPath;
        }

        /// <summary>
        /// Xóa file
        /// </summary>
        public Task DeleteAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Mở file để download
        /// </summary>
        public Task<Stream> OpenReadAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            Stream stream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);

            return Task.FromResult(stream);
        }

        /// <summary>
        /// Kiểm tra file tồn tại
        /// </summary>
        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
