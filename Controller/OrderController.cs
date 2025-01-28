using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sambackend.Dto;
using sambackend.Services;
using System.Security.Claims;

namespace sambackend.Controllers
{
    [Route("api/order")]
    [ApiController]
    [Authorize]  // Ensure authentication
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound(new { status = "Error", message = "Order not found." });

            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var createdOrder = await _orderService.CreateOrderAsync(orderDto, userId);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpPost("{id}/status")]
        public async Task<IActionResult> ConfirmOrderDelivery(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var result = await _orderService.ConfirmOrderDeliveryAsync(id, userId);
            if (!result)
                return NotFound(new { status = "Error", message = "Order not found or not authorized." });

            return Ok(new { status = "Success", message = "Order delivery confirmed." });
        }
    }
}
