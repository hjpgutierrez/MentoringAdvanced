using Catalog.Application.Common.Interfaces;
using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.MessageBrokers
{
    public class RabbitMqMessagePublisher : IMessagePublisher
    {
        private readonly ILogger<RabbitMqMessagePublisher> _logger;

        public RabbitMqMessagePublisher(ILogger<RabbitMqMessagePublisher> logger)
        {
            _logger = logger;
        }

        public async Task PublishAsync<T>(T message, string routingKey)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: routingKey, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: routingKey, body: body);
            _logger.LogInformation($"Sent {message}");
            await Task.CompletedTask;
        }
    }
}
