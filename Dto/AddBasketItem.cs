using System;

namespace sambackend.DTOs
{
    public class AddBasketItemDto
    {
        public Guid DishId { get; set; } // ✅ Only requires DishId
        public int Quantity { get; set; } // ✅ Number of items
    }
}
