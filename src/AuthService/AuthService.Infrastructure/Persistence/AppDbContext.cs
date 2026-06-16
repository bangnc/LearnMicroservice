using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        public DbSet<Unit> Units { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Unit>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });
            builder.Entity<Unit>()
                    .HasMany(x => x.Users)
                    .WithOne()
                    .HasForeignKey(x => x.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
