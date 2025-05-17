using Carting.BLL.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Carting.DAL.MessageBrokers
{
    public class RabbitMqMessageConsumer : BackgroundService
    {
        const string ProductQueue = "product";
        private readonly IServiceProvider _serviceProvider;

        public RabbitMqMessageConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: ProductQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, eventArgument) =>
            {
                var body = eventArgument.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                using var scope = _serviceProvider.CreateScope();
                var messageProcessor = scope.ServiceProvider.GetRequiredService<IMessageProcessor>();
                await messageProcessor.ProcessMessageAsync(message);

                await channel.BasicAckAsync(eventArgument.DeliveryTag, false);
            };

            await channel.BasicConsumeAsync(queue: ProductQueue, autoAck: false, consumer: consumer);

            await Task.CompletedTask;
        }
    }
}
