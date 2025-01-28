using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sambackend.Models
{
    [Table("Order")]  
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        public string deliveryTime { get; set; }
        public string OrderTime { get; set; }
        public OrderStatus status { get; set; }
        public int price { get; set; }
        public string address { get; set; }
        public string userId { get; set; }
    }
}
