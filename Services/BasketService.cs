using System;
using System.Linq;
using System.Threading.Tasks;
using sambackend.Data;
using sambackend.Models;
using Microsoft.EntityFrameworkCore;

namespace sambackend.Services
{
    public class BasketService : IBasketService
    {
        private readonly DataContext _context;

        public BasketService(DataContext context)
        {
            _context = context;
        }

        /// ✅ Fetch the basket with dish details included
        public async Task<Basket?> GetBasketByUserIdAsync(Guid userId)
        {
            return await _context.Baskets
                .Include(b => b.Items)
                .ThenInclude(i => i.Dish)
                .FirstOrDefaultAsync(b => b.UserId == userId);
        }

        /// ✅ Fix: Implement `AddItemToBasketAsync` properly
        public async Task<BasketItem> AddItemToBasketAsync(Guid basketId, Guid dishId, int quantity, decimal unitPrice)
        {
            var basket = await _context.Baskets
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.Id == basketId);

            if (basket == null)
            {
                throw new Exception("Basket not found.");
            }

            var basketItem = new BasketItem
            {
                Id = Guid.NewGuid(),
                BasketId = basketId,
                DishId = dishId,
                Quantity = quantity,
                UnitPrice = unitPrice
            };

            _context.BasketItems.Add(basketItem);
            await _context.SaveChangesAsync();
            return basketItem;
        }
        public async Task<Basket> CreateBasketForUserAsync(Guid userId)
        {
          var basket = new Basket
         {
        Id = Guid.NewGuid(),
        UserId = userId,
        Items = new List<BasketItem>()
         };

         _context.Baskets.Add(basket);
         await _context.SaveChangesAsync();
          return basket;
        }

    }
}
