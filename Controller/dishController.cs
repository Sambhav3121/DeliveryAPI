using Microsoft.AspNetCore.Mvc;
using sambackend.Data;
using sambackend;
using sambackend.Models;

namespace sambackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly DataContext _context;

        public DishController(DataContext context)
        {
            _context = context;
        }

        // Get all dishes
        [HttpGet]
        public IActionResult GetDishes()
        {
            var dishes = _context.Dishes.ToList();
            return Ok(dishes);
        }

        // Get a single dish by ID
        [HttpGet("{id}")]
        public IActionResult GetDish(Guid id)
        {
            var dish = _context.Dishes.Find(id);
            if (dish == null)
                return NotFound("Dish not found");
            return Ok(dish);
        }

        [HttpPost]
        public IActionResult CreateDish([FromBody] Dish dish)
        {
            if (dish == null || !ModelState.IsValid)
                return BadRequest("Invalid dish data");

            dish.Id = Guid.NewGuid(); // Ensure a unique ID
            _context.Dishes.Add(dish);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetDish), new { id = dish.Id }, dish);
        }

        
        [HttpPut("{id}")]
        public IActionResult UpdateDish(Guid id, [FromBody] Dish updatedDish)
        {
            var existingDish = _context.Dishes.Find(id);
            if (existingDish == null)
                return NotFound("Dish not found");

            if (!ModelState.IsValid)
                return BadRequest("Invalid dish data");

            
            existingDish.Name = updatedDish.Name;
            existingDish.Description = updatedDish.Description;
            existingDish.Price = updatedDish.Price;
            existingDish.Image = updatedDish.Image;
            existingDish.Vegetarian = updatedDish.Vegetarian;
            existingDish.Rating = updatedDish.Rating;

            _context.SaveChanges();
            return NoContent();
        }

       
        [HttpDelete("{id}")]
        public IActionResult DeleteDish(Guid id)
        {
            var dish = _context.Dishes.Find(id);
            if (dish == null)
                return NotFound("Dish not found");

            _context.Dishes.Remove(dish);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
