using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mvc_app.Models;
using mvc_app.Services;
using System.Security.Claims;

namespace mvc_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItem cartItem)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized("User not logged in.");
                }

                var userId = userIdClaim.Value;

                if (cartItem == null || cartItem.Quantity <= 0 || cartItem.ProductId <= 0)
                {
                    return BadRequest("Invalid cart item.");
                }

                cartItem.UserId = userId;

                var productExists = await _cartService.CheckIfProductExists(cartItem.ProductId);
                if (!productExists)
                {
                    return NotFound($"Product with ID {cartItem.ProductId} not found.");
                }

                var addedItem = await _cartService.AddToCart(cartItem);

                return Ok(new
                {
                    Message = "Item added to cart successfully.",
                    CartItem = addedItem
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddToCart: {ex}");
                return StatusCode(500, new
                {
                    Message = "An error occurred while adding the item to the cart.",
                    Details = ex.Message
                });
            }
        }


        [HttpGet]
        public IActionResult GetCartItems()
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new { Message = "User not logged in." });
            }
            var userId = userIdClaim.Value;

            var cartItems = _cartService.GetCartItems(userId);
            if (!cartItems.Any())
            {
                return Ok(new List<CartItem>()); // Повертаємо порожній масив
            }

            return Ok(cartItems);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Message = "Invalid cart item ID." });
            }

            var success = await _cartService.RemoveFromCart(id);
            if (!success)
            {
                return NotFound(new { Message = "Cart item not found." });
            }

            return Ok(new { Message = "Item successfully removed from the cart." });
        }
    }
}
