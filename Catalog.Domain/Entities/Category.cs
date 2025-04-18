using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class Category : AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public int? ParentCategoryId { get; set; }

        public Category? ParentCategory { get; set; }

        public ICollection<Category>? ChildCategories { get; set; }

        //public int ProductId { get; set; }
        //public Product Product { get; set; }
    }
}
