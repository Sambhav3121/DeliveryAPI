using System;
using System.Threading.Tasks;
using sambackend.Models;

namespace sambackend.Services
{
    public interface IBasketService
    {
        Task<Basket?> GetBasketByUserIdAsync(Guid userId);
        Task<BasketItem> AddItemToBasketAsync(Guid basketId, Guid dishId, int quantity, decimal unitPrice); 
        Task<Basket> CreateBasketForUserAsync(Guid userId); // ✅ New method to create a basket if it doesn't exist
    }
}
