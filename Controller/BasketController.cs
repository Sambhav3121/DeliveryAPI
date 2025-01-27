using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using sambackend.DTOs;
using sambackend.Services;
using sambackend.Models;

namespace sambackend.Controllers
{
    [ApiController]
    [Route("api/basket")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IDishService _dishService;

        public BasketController(IBasketService basketService, IDishService dishService)
        {
            _basketService = basketService;
            _dishService = dishService;
        }

        /// ✅ Get the user's full basket
        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { message = "Invalid user ID format." });

            var basket = await _basketService.GetBasketByUserIdAsync(userId);
            if (basket == null || basket.Items.Count == 0)
                return NotFound(new { message = "Basket is empty." });

            var response = basket.Items.Select(item => new
            {
                id = item.Dish.Id, // ✅ Use Dish ID
                name = item.Dish.Name, // ✅ Use Dish Name
                price = item.UnitPrice, // ✅ Dish Price
                totalPrice = item.UnitPrice * item.Quantity, // ✅ Calculated Total Price
                amount = item.Quantity, // ✅ Quantity of this Dish
                image = item.Dish.Image // ✅ Dish Image URL
            }).ToList();

            return Ok(response);
        }

        /// ✅ Add an item to the basket using only `DishId` and `Quantity`
        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] AddBasketItemDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { message = "Invalid user ID format." });

            var basket = await _basketService.GetBasketByUserIdAsync(userId);
            if (basket == null)
                return NotFound(new { message = "Basket not found for this user." });

            var dish = await _dishService.GetDishByIdAsync(dto.DishId);
            if (dish == null)
                return BadRequest(new { message = "Invalid Dish ID. Dish does not exist." });

            var basketItem = await _basketService.AddItemToBasketAsync(
                basket.Id, dto.DishId, dto.Quantity, (decimal)dish.Price);

            return CreatedAtAction(nameof(GetBasket), new { }, new
            {
                message = "Dish added to basket successfully.",
                basketItem.Id,
                basketItem.DishId,
                basketItem.Quantity,
                basketItem.UnitPrice
            });
        }

        /// ✅ New API: Add dish to basket by `dishId` (without requiring DTO)
        [HttpPost("dish/{dishId}")]
public async Task<IActionResult> AddDishToBasket(Guid dishId)
{
    var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
    
    // 🔴 Debugging: Print extracted User ID
    Console.WriteLine($"Extracted User ID from JWT: {userIdClaim}");

    if (string.IsNullOrEmpty(userIdClaim))
        return Unauthorized(new { status = "error", message = "User ID not found in token." });

    if (!Guid.TryParse(userIdClaim, out Guid userId))
        return Unauthorized(new { status = "error", message = "Invalid user ID format.", receivedValue = userIdClaim });

    var dish = await _dishService.GetDishByIdAsync(dishId);
    if (dish == null)
        return NotFound(new { status = "error", message = "Dish not found." });

    var basket = await _basketService.GetBasketByUserIdAsync(userId);
    if (basket == null)
    {
        basket = await _basketService.CreateBasketForUserAsync(userId);
    }

    var basketItem = await _basketService.AddItemToBasketAsync(
        basket.Id, dishId, 1, (decimal)dish.Price); // Default quantity is 1

    return Ok(new
    {
        status = "success",
        message = "Dish added to cart successfully.",
        basketItem.Id,
        basketItem.DishId,
        basketItem.Quantity,
        basketItem.UnitPrice
    });
}

    }
}
