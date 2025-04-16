using Carting.BLL.Models;

namespace Carting.BLL.Services
{
    public interface ICartService
    {
        Cart GetCart(string cartId);

        bool AddItem(string cartId, Item item);

        bool RemoveItem(string cartId, int itemId);
    }
}
