using Microsoft.EntityFrameworkCore;
using sambackend.Data;
using sambackend.Dto;
using sambackend.Models;

namespace sambackend.Services
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;

        public OrderService(DataContext context)
        {
            _context = context;
        }

        public async Task<OrderDto?> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
                return null;

            return new OrderDto
            {
                Id = order.Id,
                deliveryTime = order.deliveryTime,  
                orderTime = order.OrderTime,  
                status = order.status,
                price = order.price,
                address = order.address
            };
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.userId == userId)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    deliveryTime = o.deliveryTime, 
                    orderTime = o.OrderTime,  
                    status = o.status,
                    price = o.price,
                    address = o.address
                })
                .ToListAsync();
        }

        public async Task<OrderDto> CreateOrderAsync(OrderCreateDto orderDto, string userId)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                deliveryTime = orderDto.deliveryTime,  
                OrderTime = DateTime.UtcNow,  
                status = OrderStatus.Pending,  
                price = 0, 
                address = orderDto.address,
                userId = userId
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return new OrderDto
            {
                Id = order.Id,
                deliveryTime = order.deliveryTime, 
                orderTime = order.OrderTime,  
                status = order.status,
                price = order.price,
                address = order.address
            };
        }

        public async Task<bool> ConfirmOrderDeliveryAsync(Guid orderId, string userId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.userId == userId);
            if (order == null)
                return false;

            order.status = OrderStatus.Delivered;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}