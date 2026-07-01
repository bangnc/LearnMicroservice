using AuthService.Application.Common.Events;
using AuthService.Application.Common.Messaging;
using AuthService.Infrastructure.Messaging.Handlers;
using AuthService.Infrastructure.Messaging.Kafka;
using AuthService.Infrastructure.Persistence.Configurations;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;




namespace AuthService.Infrastructure.Messaging.Consumers
{
    public class SendOtpConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly KafkaSettings _kafka;

        public SendOtpConsumer(
            IOptions<KafkaSettings> kafka,
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration)
        {
            _kafka = kafka.Value;
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = _kafka.BootstrapServers,
                    GroupId = "auth-send-otp-group",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false
                };

                using var consumer = new ConsumerBuilder<string, string>(config).Build();
                consumer.Subscribe(KafkaTopics.SendOtp);

                Console.WriteLine("Consumer START");

                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        ConsumeResult<string, string>? result = null;

                        try
                        {
                            result = consumer.Consume(stoppingToken);

                            if (result?.Message?.Value == null)
                                continue;

                            using var scope = _scopeFactory.CreateScope();

                            var handler = scope.ServiceProvider
                                .GetRequiredService<SendOtpHandler>();

                            SendOtpEmailIntegrationEvent? message;

                            try
                            {
                                message = JsonSerializer.Deserialize<SendOtpEmailIntegrationEvent>(
                                    result.Message.Value,
                                    new JsonSerializerOptions
                                    {
                                        PropertyNameCaseInsensitive = true
                                    });
                            }
                            catch (JsonException jsonEx)
                            {
                                Console.WriteLine($"❌ JSON invalid: {jsonEx.Message}");
                                Console.WriteLine($"RAW: {result.Message.Value}");

                                consumer.Commit(result);
                                continue;
                            }

                            if (message == null)
                                continue;

                            await handler.Handle(message);

                            consumer.Commit(result);
                        }
                        catch (ConsumeException ex)
                        {
                            Console.WriteLine($"Kafka consume error: {ex.Error.Reason}");
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Handler error: {ex}");
                        }
                    }
                }
                finally
                {
                    consumer.Close();
                }

            }, stoppingToken);
        }
        
    }
}
