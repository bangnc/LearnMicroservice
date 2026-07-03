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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
     this IServiceCollection services,
     IConfiguration configuration)
        {
            services.AddJwtAuthentication(configuration);

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

        public static IServiceCollection AddJwtAuthentication(
       this IServiceCollection services,
       IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = configuration["Jwt:Audience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}
