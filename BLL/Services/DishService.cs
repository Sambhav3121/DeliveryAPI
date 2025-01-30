using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        if (categories != null && categories.Any())
        {
            query = query.Where(d => categories.Contains(d.Category));
        }

        if (vegetarian)
        {
            query = query.Where(d => d.Vegetarian);
        }

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

        int skip = (page - 1) * pageSize;
        var pagedDishes = await query.Skip(skip).Take(pageSize).ToListAsync();

        return pagedDishes;
    }

    public async Task<Dish?> GetDishByIdAsync(Guid dishId)
    {
        return await _context.Dishes.FindAsync(dishId);
    }

    public async Task<bool> CanUserRateDishAsync(Guid dishId, string userId)
    {
        var dishExists = await _context.Dishes.AnyAsync(d => d.Id == dishId);
        if (!dishExists)
            return false;

        var userHasOrderedDish = await _context.Orders
            .Include(o => o.userId)
            .AnyAsync(o => o.userId == userId);

        return userHasOrderedDish;
    }

    public async Task<bool> RateDishAsync(Guid dishId, string userId, int ratingScore)
    {
        var dish = await _context.Dishes.FindAsync(dishId);
        if (dish == null)
            return false;

        var previousRating = await _context.Dishes
            .Where(d => d.Id == dishId)
            .Select(d => d.Rating)
            .FirstOrDefaultAsync();

        dish.Rating = previousRating == 0 ? ratingScore : (previousRating + ratingScore) / 2;
        
        await _context.SaveChangesAsync();
        return true;
    }
}