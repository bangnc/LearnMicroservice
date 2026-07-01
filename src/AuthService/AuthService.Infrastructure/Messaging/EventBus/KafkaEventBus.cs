using AuthService.Application.Common.Events;
using AuthService.Application.Common.Messaging;
using AuthService.Infrastructure.Messaging.Kafka;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Messaging.EventBus
{
    public class KafkaEventBus : IEventBus
    {
        private readonly IProducer<string, string> _producer;

        public KafkaEventBus(IConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishAsync<T>(
            string topic,
            T @event,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(topic))
                throw new ArgumentException("Kafka topic is required");

            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var message = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonSerializer.Serialize(@event)
            };

            try
            {
                await _producer.ProduceAsync(topic, message, cancellationToken);
            }
            catch (ProduceException<string, string> ex)
            {
                // log sau (Serilog/ILogger)
                throw new Exception($"Kafka publish failed: {ex.Error.Reason}", ex);
            }
        }
    }
}
