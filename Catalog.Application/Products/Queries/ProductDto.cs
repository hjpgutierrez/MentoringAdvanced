namespace Catalog.Application.Products.Queries
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Image { get; set; }

        public int Amount { get; set; }

        public decimal Price { get; set; }

        public string Href { get; set; }
    }
}
