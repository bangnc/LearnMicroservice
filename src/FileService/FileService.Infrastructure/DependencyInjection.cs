using FileService.Application.Interfaces;
using FileService.Domain.Repositories;
using FileService.Infrastructure.Persistence;
using FileService.Infrastructure.Repositories;
using FileService.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<FileDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IFileRepository, FileRepository>();

            services.AddScoped<IStorageService, LocalStorageService>();

            return services;
        }
    }
}
