using Microsoft.AspNetCore.Mvc;
using sambackend.Data;
using sambackend;
using sambackend.Models;
using sambackend.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace sambackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IDishService _dishService;


        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        // Get all dishes


          [HttpGet]
[SwaggerOperation(Summary = "Get a list of dishes (menu) with filtering, sorting, and pagination.")]
[SwaggerResponse(200, "Success", typeof(List<Dish>))]
[SwaggerResponse(400, "Invalid parameters.")]
public async Task<IActionResult> GetDishes(
    [FromQuery, SwaggerParameter("Available values: Wok, Pizza, Soup, Dessert, Drink.")] List<DishCategory>? categories = null,
    [FromQuery, SwaggerParameter("Filter dishes by vegetarian option (default: false).")] bool vegetarian = false,
    [FromQuery, SwaggerParameter("Available values: NameAsc, NameDesc, PriceAsc, PriceDesc, RatingAsc, RatingDesc.")] string sorting = "NameAsc",
    [FromQuery, SwaggerParameter("Specify the page number for pagination (default: 1).")] int page = 1,
    [FromQuery] int pageSize = 5)
{
        {
            // Validate sorting parameter
            var validSortOptions = new[] { "NameAsc", "NameDesc", "PriceAsc", "PriceDesc", "RatingAsc", "RatingDesc" };
            if (!validSortOptions.Contains(sorting))
                return BadRequest("Invalid sorting option.");

            // Call the DishService to retrieve the filtered and sorted list of dishes
            var dishes = await _dishService.GetDishesAsync(categories, vegetarian, sorting, page, pageSize);

            return Ok(dishes); // Return the dishes as a JSON response
        } 
    
             }
    }
}

            // Get a single dish by ID
   /*     [HttpGet("{id}")]
        public async Task<IActionResult> GetDish(Guid id)
        {
            var dish = _dishService.Dishes.Find(id);
            if (dish == null)
                return NotFound("Dish not found");
            return Ok(dish);
        }
        
        

    }
}
*/