﻿using Carting.BLL.Models;

namespace Carting.BLL.Interfaces
{
    public interface ICartService
    {
        Cart? GetCart(string cartId);

        bool AddItem(string cartId, Item item);

        bool RemoveItem(string cartId, int itemId);

        IList<Cart> GetDocumentsByItemId(int itemId);

        bool UpdateDocument(Cart item);
    }
}
