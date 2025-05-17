using Carting.BLL.DTOs;
using Carting.BLL.Interfaces;
using Carting.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carting.Controllers.V1
{
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET api/<CartController>/5
        [HttpGet("{id}")]
        public ActionResult<Cart> Get(string id)
        {
            Cart cart = _cartService.GetCart(id);
            if (cart == null || string.IsNullOrEmpty(cart?.Id))
            { 
                return NotFound();
            }

            var dto = new CartReponseDtoV1(cart.Code, cart.Items);
            return Ok(dto);
        }

        // POST api/<CartController>/5
        [HttpPost("{id}")]
        public IActionResult Post(string id, [FromBody] Item value)
        {        
            return _cartService.AddItem(id, value) ? Ok() : BadRequest();
        }

        // DELETE api/<CartController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id, [FromBody] int itemId)
        {
            return _cartService.RemoveItem(id, itemId) ? Ok() : BadRequest();
        }
    }
}
