using  sambackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sambackend.Dto
{
    public class OrderDto
    {
        public Guid Id {get; set;}
        public string deliveryTime { get; set; }
        public string orderTime { get; set; }
        public OrderStatus status { get; set; }
        public int price { get; set; }
        public string address { get; set; }
    }
}