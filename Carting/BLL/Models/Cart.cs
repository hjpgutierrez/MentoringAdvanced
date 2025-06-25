using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Carting.BLL.Models
{
    public class Cart : EntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public IList<Item> Items;

        public Cart()
        {
            Items = new List<Item>();
        }

        public IList<Item> GetItems() => Items;

        public Item GetItem(int itemId) => Items.First(i => i.Id == itemId);

        public bool AddItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (Items.Any(i => i.Id == item.Id))
            {
                return false;
            }

            Items.Add(item);
            return true;
        }

        public bool RemoveItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (!Items.Any(i => i.Id == item.Id))
            {
                return false;
            }

            Items.Remove(item);
            return true;
        }
    }
}
