using AuthService.Application.Common.Messaging;
using AuthService.Application.Interfaces;
using AuthService.Application.Services;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Messaging.Consumers;
using AuthService.Infrastructure.Messaging.EventBus;
using AuthService.Infrastructure.Messaging.Handlers;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Persistence.Configurations;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AuthService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
     this IServiceCollection services,
     IConfiguration configuration)
        {
            services.AddScoped<IAuthServiceManager, AuthServiceManager>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.Configure<EmailSettings>(
                configuration.GetSection("EmailSettings"));

            services.Configure<KafkaSettings>(
               configuration.GetSection("Kafka"));

            services.AddScoped<IEmailService, EmailService>();
            // Kafka

            services.AddSingleton<IEventBus, KafkaEventBus>();

            services.AddScoped<SendOtpHandler>();

            services.AddHostedService<SendOtpConsumer>();

            // Repositories
            services.AddScoped<ILoginSessionRepository, LoginSessionRepository>();

            return services;
        }
    }
}
