using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Products.EventHandlers;
public class ProductChangedEventHandler : INotificationHandler<ProductChangedEvent>
{
    private readonly ILogger<ProductChangedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public ProductChangedEventHandler(ILogger<ProductChangedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(ProductChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().Name);

        await _messagePublisher.PublishAsync(notification, "product");

        await Task.CompletedTask;
    }
}

