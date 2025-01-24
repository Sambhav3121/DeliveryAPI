using System.Linq;
using Microsoft.EntityFrameworkCore;
using sambackend.Data;
using sambackend.Models;
using sambackend.Services;

public class DishService : IDishService
{
    private readonly DataContext _context;

    public DishService(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Dish>> GetDishesAsync(
        List<DishCategory>? categories, 
        bool vegetarian, 
        SortingOption sorting, 
        int page, 
        int pageSize)
    {
        IQueryable<Dish> query = _context.Dishes;

        // Filter by categories if provided
        if (categories != null && categories.Any())
        {
            query = query.Where(d => categories.Contains(d.Category));
        }

        // Filter by vegetarian option
        if (vegetarian)
        {
            query = query.Where(d => d.Vegetarian);
        }

        // Apply sorting
        query = sorting switch
        {
            SortingOption.NameAsc => query.OrderBy(d => d.Name),
        SortingOption.NameDesc => query.OrderByDescending(d => d.Name),
        SortingOption.PriceAsc => query.OrderBy(d => d.Price),
        SortingOption.PriceDesc => query.OrderByDescending(d => d.Price),
        SortingOption.RatingAsc => query.OrderBy(d => d.Rating),
        SortingOption.RatingDesc => query.OrderByDescending(d => d.Rating),
        _ => query.OrderBy(d => d.Name)
        };

        // Apply pagination
        int skip = (page - 1) * pageSize;
        var pagedDishes = await query.Skip(skip).Take(pageSize).ToListAsync();

        return pagedDishes;
    }
    public async Task<Dish?> GetDishByIdAsync(Guid id)
{
    // Find the dish by ID in the database
    return await _context.Dishes.FirstOrDefaultAsync(d => d.Id == id);
}

}
