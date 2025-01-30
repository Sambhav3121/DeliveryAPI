using Microsoft.EntityFrameworkCore;
using sambackend.Data;
using sambackend.Dto;
using sambackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sambackend.Services
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;

        public CartService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<CartDto>> GetCartItemsAsync(string userId)
        {
            return await _context.Carts
                .Where(c => c.UserId == userId)
                .Select(c => new CartDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Price = c.Price,
                    TotalPrice = c.TotalPrice,
                    Amount = c.Amount,
                    Image = c.Image
                })
                .ToListAsync();
        }

        public async Task AddToCartAsync(Guid dishId, string userId)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
            if (dish == null) return;

           var existingCartItem = await _context.Carts
    .FirstOrDefaultAsync(c => c.UserId == userId && c.dishId.ToString() == dishId.ToString());


            if (existingCartItem == null)
            {
                var newCartItem = new Cart
                {
                    Id = Guid.NewGuid(),
                    Name = dish.Name,
                    Price = dish.Price,
                    TotalPrice = dish.Price,
                    Amount = 1,
                    Image = dish.Image,
                    UserId = userId,
                    dishId = dishId  
                };

                await _context.Carts.AddAsync(newCartItem);
            }
            else
            {
                existingCartItem.Amount++;
                existingCartItem.TotalPrice = existingCartItem.Price * existingCartItem.Amount;
            }

            await _context.SaveChangesAsync();
        }

        
        public async Task<bool> RemoveFromCartAsync(Guid dishId, string userId, bool increase)
        {
            var cartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.dishId.ToString() == dishId.ToString()); 

            if (cartItem == null) return false;

            if (increase)
            {
                cartItem.Amount--;
                cartItem.TotalPrice = cartItem.Price * cartItem.Amount;

                if (cartItem.Amount <= 0)
                    _context.Carts.Remove(cartItem);
            }
            else
            {
                _context.Carts.Remove(cartItem);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        
        public async Task<bool> DoesDishExistAsync(Guid dishId)
        {
            return await _context.Dishes.AnyAsync(d => d.Id == dishId);
        }

       
        public async Task<bool> ClearCartAsync(string userId)
        {
            var cartItems = await _context.Carts.Where(c => c.UserId == userId).ToListAsync();

            if (!cartItems.Any())
                return false;

            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
