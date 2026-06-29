using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Persistence.Configurations
{
    public class LoginSessionConfiguration : IEntityTypeConfiguration<LoginSession>
    {
        public void Configure(EntityTypeBuilder<LoginSession> entity)
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.SessionId)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.UserId)
                .IsRequired();

            entity.Property(x => x.Otp)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(x => x.ExpiredAt)
                .IsRequired();

            entity.Property(x => x.AttemptCount)
                .IsRequired();

            entity.Property(x => x.IsVerified)
                .IsRequired();

            entity.HasOne(x => x.User)
                .WithMany(u => u.LoginSessions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => x.SessionId).IsUnique();
            entity.HasIndex(x => x.UserId);
        }
    }
}
