using FileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Infrastructure.Persistence
{
    public class FileDbContext : DbContext
    {
        public FileDbContext(DbContextOptions<FileDbContext> options)
            : base(options)
        {
        }

        public DbSet<FileMetadata> Files => Set<FileMetadata>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FileMetadata>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.FileName)
                    .HasMaxLength(255);

                entity.Property(x => x.OriginalFileName)
                    .HasMaxLength(255);

                entity.Property(x => x.Extension)
                    .HasMaxLength(20);

                entity.Property(x => x.ContentType)
                    .HasMaxLength(100);

                entity.Property(x => x.FilePath)
                    .HasMaxLength(500);
            });
        }
    }
}
