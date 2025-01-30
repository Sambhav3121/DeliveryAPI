using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sambackend.Models
{
    [Table("Order")]  
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime deliveryTime { get; set; }
        public DateTime OrderTime { get; set; }
        public OrderStatus status { get; set; }
        public int price { get; set; }
        public string address { get; set; }
        public string userId { get; set; }
    }
}
