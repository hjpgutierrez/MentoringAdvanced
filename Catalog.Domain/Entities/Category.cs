using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class Category : AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public int ParentCategory { get; set; }
    }
}
