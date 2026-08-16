using AuthService.Application.Common.Events;
using AuthService.Application.Common.Messaging;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Messaging.EventBus
{
    public class OutboxProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OutboxProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

                    var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

                    var messages = await repository.GetPendingAsync(stoppingToken);

                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    foreach (var message in messages)
                    {
                        await ProcessMessageAsync(
                            message,
                            eventBus,
                            unitOfWork,
                            stoppingToken);
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine(
                   $"Outbox processing error: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
        private async Task ProcessMessageAsync(
                 OutboxMessage message,
                 IEventBus eventBus,
                 IUnitOfWork unitOfWork,
                 CancellationToken cancellationToken)
        {
            try
            {
                switch (message.EventType)
                {
                    case "UserRegistered":

                        var @event =
                            JsonSerializer.Deserialize<UserRegisteredIntegrationEvent>(
                                message.Payload);

                        if (@event == null)
                        {
                            throw new InvalidOperationException(
                                $"Invalid payload: {message.Id}");
                        }

                        await eventBus.PublishAsync(
                            KafkaTopics.UserRegistered,
                            @event,
                            cancellationToken);

                        message.ProcessedAt = DateTime.UtcNow;

                        await unitOfWork.SaveChangesAsync(cancellationToken);

                        break;

                    default:
                        throw new InvalidOperationException(
                            $"Unsupported event type: {message.EventType}");
                }
            }
            catch (Exception ex)
            {
                message.RetryCount++;
                message.Error = ex.Message;

                if (message.RetryCount > 5)
                {
                    message.FailedAt = DateTime.UtcNow;
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);

                Console.WriteLine(
                    $"Outbox retry {message.RetryCount} failed. " +
                    $"MessageId: {message.Id}, Error: {ex.Message}");
            }
        }

    }
}
