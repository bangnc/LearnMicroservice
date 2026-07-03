using FileService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain.Repositories
{
    public interface IFileRepository
    {
        Task<FileMetadata?> GetByIdAsync(Guid id);

        Task<IEnumerable<FileMetadata>> GetAllAsync();

        Task AddAsync(FileMetadata file);

        Task UpdateAsync(FileMetadata file);

        Task DeleteAsync(FileMetadata file);

        Task SaveChangesAsync();
    }
}
