using CleanArchitecture.Domain.Common;

namespace Catalog.Domain.Events;
public class ProductChangedEvent : BaseEvent
{
    public Product Product { get; }

    public ProductChangedEvent(Product product)
    {
        Product = product;
    }
}
