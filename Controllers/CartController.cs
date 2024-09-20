using Microsoft.AspNetCore.Mvc;
using web_service.Models;
using web_service.Services;

namespace web_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        // GET: api/Cart
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var carts = await _cartService.GetAsync();
                return Ok(carts);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while retrieving carts.", error = ex.Message });
            }
        }

        // GET: api/Cart/{id}
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var cart = await _cartService.GetAsync(id);

                if (cart == null)
                {
                    return NotFound(new { message = "Cart not found." });
                }

                return Ok(cart);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while retrieving the cart.", error = ex.Message });
            }
        }

        // POST: api/Cart
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cart newCart)
        {
            newCart.CartId = null; // Ensure CartId is autogenerated

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return BadRequest(new { message = "Validation failed.", errors });
            }

            try
            {
                await _cartService.CreateAsync(newCart);
                return Ok(new { message = "Cart created successfully.", cart = newCart });
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while creating the cart.", error = ex.Message });
            }
        }

        // PUT: api/Cart/{id}
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Cart updatedCart)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return BadRequest(new { message = "Validation failed.", errors });
            }

            try
            {
                var cart = await _cartService.GetAsync(id);

                if (cart == null)
                {
                    return NotFound(new { message = "Cart not found." });
                }

                updatedCart.CartId = cart.CartId; // Keep the original CartId

                await _cartService.UpdateAsync(id, updatedCart);

                return Ok(new { message = "Cart updated successfully.", cart = updatedCart });
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while updating the cart.", error = ex.Message });
            }
        }

        // DELETE: api/Cart/{id}
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var cart = await _cartService.GetAsync(id);

                if (cart == null)
                {
                    return NotFound(new { message = "Cart not found." });
                }

                await _cartService.RemoveAsync(id);

                return Ok(new { message = "Cart deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while deleting the cart.", error = ex.Message });
            }
        }
    }
}
