using Catalog.Domain.Events;

namespace Catalog.Domain.Entities
{
    public class Product : AuditableEntity
    {
        private string _name = string.Empty;
        public required string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    AddDomainEvent(new ProductChangedEvent(this));
                }

                _name = value;
            }
        }

        public string? Description { get; set; }

        private string? _image;
        public string? Image
        {
            get => _image;
            set
            {
                if (value != _image)
                {
                    AddDomainEvent(new ProductChangedEvent(this));
                }

                _image = value;
            }
        }


        public int CategoryId { get; set; }
        public Category? Category { get; set; }


        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                if (value != _price)
                {
                    AddDomainEvent(new ProductChangedEvent(this));
                }

                _price = value;
            }
        }

        public int Amount { get; set; }
    }
}
