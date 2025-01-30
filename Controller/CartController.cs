using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using sambackend.Services;
using sambackend.Dto;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace sambackend.Controllers
{
    [ApiController]
    [Route("api/basket")]
    [Authorize] 
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated." });

            var cartItems = await _cartService.GetCartItemsAsync(userId);
            return Ok(new { status = "success", cart = cartItems });
        }

        
        [HttpPost("dish/{dishId}")]
        public async Task<IActionResult> AddToCart(Guid dishId)
        {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(userId))
        return Unauthorized(new { message = "User not authenticated." });

    bool dishExists = await _cartService.DoesDishExistAsync(dishId);
    if (!dishExists)
        return NotFound(new { message = "Dish not found." });

    await _cartService.AddToCartAsync(dishId, userId);  
    return Ok(new { status = "success", message = "Dish added to cart." });
        }


        [HttpDelete("dish/{dishId}")]
        public async Task<IActionResult> RemoveFromCart(Guid dishId, [FromQuery] bool increase = false)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated." });

            bool result = await _cartService.RemoveFromCartAsync(dishId, userId, increase);
            if (!result)
                return NotFound(new { message = "Item not found in cart." });

            return Ok(new { status = "success", message = increase ? "Item quantity decreased." : "Item removed from cart." });
        }
    }
}