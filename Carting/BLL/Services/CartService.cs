using Carting.BLL.Models;
using Carting.BLL.Validations;
using Carting.DAL;

namespace Carting.BLL.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<Cart> _repository;

        public CartService(IRepository<Cart> repository)
        {
            _repository = repository;
        }

        public bool AddItem(string cartId, Item item)
        {
            if (string.IsNullOrWhiteSpace(cartId)) { 
                return false;
            }

            if (!new ItemValidator().Validate(item).IsValid)
            {
                return false;
            }

            Cart cart = _repository.GetDocumentById(cartId);
            if (cart == null || string.IsNullOrEmpty(cart?.Id))
            {
                cart = new Cart();
                cart.Code = cartId;
                _repository.InsertDocument(cart);
            }

            if (!cart.AddItem(item))
            {
                return false;
            }

            _repository.UpdateDocument(cart);
            return true;
        }

        public Cart? GetCart(string cartId)
        {
            if (string.IsNullOrWhiteSpace(cartId))
            {
                return null;
            }

            return _repository.GetDocumentById(cartId);
        }

        public bool RemoveItem(string cartId, int itemId)
        {
            if (string.IsNullOrWhiteSpace(cartId))
            {
                return false;
            }

            if (itemId < 0) 
            {
                return false;
            }

            Cart cart = _repository.GetDocumentById(cartId);
            if (cart == null)
            {
                return false;
            }

            Item itemToDelete = cart.GetItem(itemId);
            if (itemToDelete == null) { 
                return false;
            }

            if (!cart.RemoveItem(itemToDelete))
            {
                return false;
            }

            return _repository.UpdateDocument(cart);
        }
    }
}
