﻿namespace Catalog.Domain.Entities
{
    public class Category : AuditableEntity
    {
        public required string Name { get; set; }

        public string? Image { get; set; }

        public int? ParentCategoryId { get; set; }

        public Category? ParentCategory { get; set; }

        public ICollection<Category>? ChildCategories { get; set; }

        public ICollection<Product>? ChildProducts { get; set; }
    }
}
