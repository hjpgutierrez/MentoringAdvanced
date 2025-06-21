using Carting.BLL.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Carting.DAL.MessageBrokers;
public class RabbitMqMessageConsumer : BackgroundService
{
    const string ProductQueue = "product";
    private readonly IServiceProvider _serviceProvider;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqMessageConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _connection = null;
        _channel = null;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        await _channel.QueueDeclareAsync(queue: ProductQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += OnMessageRecieved;
        await _channel.BasicConsumeAsync(queue: ProductQueue, autoAck: false, consumer: consumer);
        await Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _channel?.Dispose();
        _connection?.Dispose();
        return Task.CompletedTask;
    }

    private async Task OnMessageRecieved(object model, BasicDeliverEventArgs eventArgument)
    {
        using var scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<RabbitMqMessageConsumer>>();
        try
        {
            var messageProcessor = scope.ServiceProvider.GetRequiredService<IMessageProcessor>();

            var body = eventArgument.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            logger.LogInformation($"Received message: {message}");

            await messageProcessor.ProcessMessageAsync(message);
            logger.LogInformation($"message processed: {message}");

            // Acknowledge the message
            await _channel!.BasicAckAsync(eventArgument.DeliveryTag, false);
            logger.LogInformation($"Message acknowledged: {message}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error processing message: {ex.Message}");
            // Optionally reject the message
            await _channel!.BasicNackAsync(eventArgument.DeliveryTag, false, requeue: true);
        }
    }

}