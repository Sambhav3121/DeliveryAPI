using sambackend.Dto;

namespace sambackend.Services
{
    public interface IOrderService
    {
        Task<OrderDto?> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(string userId);
        Task<OrderDto> CreateOrderAsync(OrderCreateDto orderDto, string userId);
        Task<bool> ConfirmOrderDeliveryAsync(Guid orderId, string userId);
        Task<bool> CancelOrderAsync(Guid orderId);  // Added CancelOrderAsync
    }
}
