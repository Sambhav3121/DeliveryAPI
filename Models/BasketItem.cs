using System;

namespace sambackend.Models
{
    public class BasketItem
    {
        public Guid Id { get; set; }
        public Guid BasketId { get; set; }
        public Guid DishId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Basket Basket { get; set; }
        public Dish Dish { get; set; }
    }
}
