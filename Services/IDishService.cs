using sambackend.Models;

namespace sambackend.Services
{
    public interface IDishService
    {
        Task<IEnumerable<Dish>> GetDishesAsync(List<DishCategory>? categories, bool vegetarian, string sorting, int page, int pageSize);
    }
}
