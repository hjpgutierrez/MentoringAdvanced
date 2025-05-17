namespace Catalog.Application.Common.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(T message, string routingKey);
    }
}
