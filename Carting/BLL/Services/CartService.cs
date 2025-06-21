using Carting.BLL.Interfaces;
using Carting.BLL.Models;
using Carting.BLL.Validations;

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
            // If there was no cart for specified key – creates it.
            if (cart == null || string.IsNullOrEmpty(cart?.Id))
            {
                cart = new Cart() { Code = cartId };
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

        public IList<Cart> GetDocumentsByItemId(int itemId)
        {
            if (itemId < 0)
            {
                throw new ArgumentException("Must be greater than 0", nameof(itemId));
            }

            return _repository.GetDocumentsByItemId(itemId.ToString());
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

        public bool UpdateDocument(Cart item)
        {
            return _repository.UpdateDocument(item);
        }
    }
}
