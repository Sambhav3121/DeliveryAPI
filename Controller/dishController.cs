using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sambackend.Data;
using sambackend.Models;
using sambackend.Services;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;

namespace sambackend.Controllers
{
    [ApiController]
    [Route("api/dish")]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get a list of dishes with filtering, sorting, and pagination.")]
        [SwaggerResponse(200, "Success", typeof(List<Dish>))]
        public async Task<IActionResult> GetDishes(
            [FromQuery] List<DishCategory>? categories = null,
            [FromQuery] bool vegetarian = false,
            [FromQuery] SortingOption sorting = SortingOption.NameAsc,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            var dishes = await _dishService.GetDishesAsync(categories, vegetarian, sorting, page, pageSize);
            return Ok(dishes);
        }

        [HttpGet("{id:guid}")]
        [SwaggerOperation(Summary = "Get a single dish by its ID.")]
        [SwaggerResponse(200, "Dish found.", typeof(Dish))]
        [SwaggerResponse(404, "Dish not found.")]
        public async Task<IActionResult> GetDishById([FromRoute] Guid id)
        {
            var dish = await _dishService.GetDishByIdAsync(id);
            if (dish == null)
            {
                return NotFound(new { status = "Error", message = "Dish not found." });
            }
            return Ok(dish);
        }
        
       [HttpGet("{id:guid}/rating/check")]
       [Authorize]
       [SwaggerOperation(Summary = "Check if the user can rate a dish.")]
       [SwaggerResponse(200, "Success", typeof(bool))]
      public async Task<IActionResult> CanUserRateDish([FromRoute] Guid id)
        {
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (userId == null)
        return Unauthorized();

    bool canRate = await _dishService.CanUserRateDishAsync(id, userId);
    return Ok(new { canRate });
     }

      [HttpPost("{id:guid}/rating")]
      [Authorize]
      [SwaggerOperation(Summary = "Set a rating for a dish.")]
       [SwaggerResponse(200, "Rating added successfully.")]
       [SwaggerResponse(400, "Invalid rating value.")]
      public async Task<IActionResult> RateDish([FromRoute] Guid id, [FromQuery] int ratingScore)
    {
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (userId == null)
        return Unauthorized();

    if (ratingScore < 1 || ratingScore > 5)
        return BadRequest(new { status = "Error", message = "Rating must be between 1 and 5." });

    var success = await _dishService.RateDishAsync(id, userId, ratingScore);
    if (!success)
        return NotFound(new { status = "Error", message = "Dish not found or already rated." });

    return Ok(new { status = "Success", message = "Rating added successfully." });
    }

    }
}
