using System;
using System.Collections.Generic;

namespace sambackend.Models
{
    public class Basket
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } // Links basket to a user
        public User User { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>(); // List of items in the basket
    }
}
