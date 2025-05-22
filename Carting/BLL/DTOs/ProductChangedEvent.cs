namespace Carting.BLL.DTOs
{
    public class ProductDetailsChangedEvent
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Image { get; set; } = null!;

        public decimal Price { get; set; }
    }

    public class ProductChangedEvent
    {
        public required ProductDetailsChangedEvent Product { get; set; }
    }
}
