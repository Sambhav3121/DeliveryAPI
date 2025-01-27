using sambackend.Models;

namespace sambackend.Services
{
    public interface IDishService
    {
        Task<IEnumerable<Dish>> GetDishesAsync(List<DishCategory>? categories, bool vegetarian, SortingOption sorting, int page, int pageSize);
        Task<Dish?> GetDishByIdAsync(Guid dishId);

    }
}
