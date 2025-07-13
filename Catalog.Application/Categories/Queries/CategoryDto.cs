namespace Catalog.Application.Categories.Queries
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Image { get; set; }

        public int? ParentCategoryId { get; set; }

        public string? Href { get; set; }
    }
}
