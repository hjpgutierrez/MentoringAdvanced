namespace Catalog.Domain.Entities
{
    public class Product : AuditableEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int CategoryId { get; set; }
        public required Category Category { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}
