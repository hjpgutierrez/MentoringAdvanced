namespace Carting.BLL.DTOs
{
    public class ProductChangedEvent
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Image { get; set; } = null!;

        public decimal Price { get; set; }
    }
}
