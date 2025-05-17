using Carting.BLL.DTOs;
using Carting.BLL.Interfaces;
using Carting.BLL.Models;
using System.Text.Json;

namespace Carting.BLL.Services
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly ILogger<MessageProcessor> _logger;
        private readonly ICartService _cartService;

        public MessageProcessor(ILogger<MessageProcessor> logger, ICartService cartService)
        {
            _logger = logger;
            _cartService = cartService;
        }

        public Task ProcessMessageAsync(string message)
        {
            _logger.LogInformation($"Processing message: {message}");

            var productEvent = JsonSerializer.Deserialize<ProductChangedEvent>(message);

            var carts = _cartService.GetDocumentsByItemId(productEvent!.Id);

            foreach (var cart in carts) {
                _logger.LogInformation($"Cart code: {cart.Code}");
                var itemToChange = cart.GetItem(productEvent.Id);
                itemToChange.Name = productEvent.Name;
                itemToChange.Price = productEvent.Price;
                itemToChange.Image = productEvent.Image;
                _cartService.UpdateDocument(cart);
            }

            _logger.LogInformation($"Carts modified: {carts.Count}");

            return Task.CompletedTask;
        }
    }
}
