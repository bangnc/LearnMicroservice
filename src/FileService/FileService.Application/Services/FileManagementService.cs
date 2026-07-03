using FileService.Application.DTOs;
using FileService.Application.Interfaces;
using FileService.Domain.Entities;
using FileService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Application.Services
{
    public class FileManagementService : IFileService
    {
        private readonly IFileRepository _repository;
        private readonly IStorageService _storageService;

        public FileManagementService(
            IFileRepository repository,
            IStorageService storageService)
        {
            _repository = repository;
            _storageService = storageService;
        }

        public async Task<FileResponse> UploadAsync(UploadFileRequest request)
        {
            var file = request.File;

            if (file == null || file.Length == 0)
                throw new Exception("File không hợp lệ.");

            // Sinh tên file mới
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            // Lưu file xuống ổ cứng
            var filePath = await _storageService.SaveAsync(file, fileName);

            // Tạo Entity
            var entity = new FileMetadata
            {
                Id = Guid.NewGuid(),
                FileName = fileName,
                OriginalFileName = file.FileName,
                Extension = Path.GetExtension(file.FileName),
                ContentType = file.ContentType,
                Size = file.Length,
                FilePath = filePath,
                CreatedAt = DateTime.UtcNow
            };

            // Lưu Database
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            return new FileResponse
            {
                Id = entity.Id,
                FileName = entity.FileName,
                OriginalFileName = entity.OriginalFileName,
                ContentType = entity.ContentType,
                Size = entity.Size,
                Url = $"/uploads/{entity.FileName}"
            };
        }

        public async Task<FileResponse?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return new FileResponse
            {
                Id = entity.Id,
                FileName = entity.FileName,
                OriginalFileName = entity.OriginalFileName,
                ContentType = entity.ContentType,
                Size = entity.Size,
                Url = $"/uploads/{entity.FileName}"
            };
        }

        public async Task<IEnumerable<FileResponse>> GetAllAsync()
        {
            var files = await _repository.GetAllAsync();

            return files.Select(x => new FileResponse
            {
                Id = x.Id,
                FileName = x.FileName,
                OriginalFileName = x.OriginalFileName,
                ContentType = x.ContentType,
                Size = x.Size,
                Url = $"/uploads/{x.FileName}"
            });
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                throw new Exception("Không tìm thấy file.");

            // Xóa file vật lý
            await _storageService.DeleteAsync(entity.FilePath);

            // Xóa metadata
            await _repository.DeleteAsync(entity);
            await _repository.SaveChangesAsync();
        }
    }
}
