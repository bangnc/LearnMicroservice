using AuthService.Application.Interfaces;
using AuthService.Application.Services;
using AuthService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Add infrastructure services here (e.g., database context, repositories, etc.)
            services.AddScoped<IAuthServiceManager, AuthServiceManager>();
            services.AddScoped<IJwtService, JwtService>();
            return services;
        }
    }
}
