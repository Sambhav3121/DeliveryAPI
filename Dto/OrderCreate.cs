using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sambackend.Dto
{
	public class OrderCreateDto
	{
        public string deliveryTime { get; set; }
        public string address { get; set; }
    }
}