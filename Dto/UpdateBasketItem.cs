using System;

namespace sambackend.DTOs
{
    public class UpdateBasketItemDto
    {
        public Guid BasketItemId { get; set; }
        public int Quantity { get; set; }
    }
}
