using FileService.Domain.Entities;
using FileService.Domain.Repositories;
using FileService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace FileService.Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly FileDbContext _context;

        public FileRepository(FileDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(FileMetadata file)
        {
            await _context.Files.AddAsync(file);
        }

        public async Task DeleteAsync(FileMetadata file)
        {
            _context.Files.Remove(file);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<FileMetadata>> GetAllAsync()
        {
            return await _context.Files
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<FileMetadata?> GetByIdAsync(Guid id)
        {
            return await _context.Files
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(FileMetadata file)
        {
            _context.Files.Update(file);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
