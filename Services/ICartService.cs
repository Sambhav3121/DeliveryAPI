using sambackend.Dto;
using sambackend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sambackend.Services
{
    public interface ICartService
    {
        Task<List<CartDto>> GetCartItemsAsync(string userId);
        Task AddToCartAsync(Guid dishId, string userId);
        Task<bool> RemoveFromCartAsync(Guid dishId, string userId, bool increase);  
        Task<bool> ClearCartAsync(string userId);
        Task<bool> DoesDishExistAsync(Guid dishId);
    }
}
