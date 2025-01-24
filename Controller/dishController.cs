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
    [FromQuery, SwaggerParameter("Available values: NameAsc, NameDesc, PriceAsc, PriceDesc, RatingAsc, RatingDesc.")] SortingOption sorting = SortingOption.NameAsc,
    [FromQuery, SwaggerParameter("Specify the page number for pagination (default: 1).")] int page = 1,
    [FromQuery] int pageSize = 5)
{
        {
            
            var dishes = await _dishService.GetDishesAsync(categories, vegetarian, sorting, page, pageSize);

            return Ok(dishes); 
        } 
    
             }
    [HttpGet("{id:guid}")]
[SwaggerOperation(Summary = "Get a single dish by its ID.")]
[SwaggerResponse(200, "Dish found.", typeof(Dish))]
[SwaggerResponse(404, "Dish not found.")]
public async Task<IActionResult> GetDishById([FromRoute] Guid id)
{
    // Call the service to retrieve the dish by ID
    var dish = await _dishService.GetDishByIdAsync(id);

    if (dish == null)
    {
        // Return 404 if the dish is not found
        return NotFound("Dish not found."); 
    }

    // Return the dish if found
    return Ok(dish);
}

   
   
    }
}

    
        
        