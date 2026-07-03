using FileService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Application.Interfaces
{
    public interface IFileService
    {
        Task<FileResponse> UploadAsync(UploadFileRequest request);

        Task<FileResponse?> GetByIdAsync(Guid id);

        Task<IEnumerable<FileResponse>> GetAllAsync();

        Task DeleteAsync(Guid id);
    }
}
