using System.Text;
using System.Text.Json;
using Catalog.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Catalog.Infrastructure.MessageBrokers
{
    public class RabbitMqMessagePublisher : IMessagePublisher
    {
        private readonly ILogger<RabbitMqMessagePublisher> _logger;
        private string _hostName;

        public RabbitMqMessagePublisher(ILogger<RabbitMqMessagePublisher> logger, IOptions<MessageBrokerSettings> options)
        {
            _logger = logger;
            _hostName = options.Value.HostName;
        }

        public async Task PublishAsync<T>(T message, string routingKey)
        {
            var factory = new ConnectionFactory { HostName = _hostName };
            using var connection = await factory.CreateConnectionAsync();
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
