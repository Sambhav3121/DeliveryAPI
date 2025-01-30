using System.ComponentModel.DataAnnotations;

namespace sambackend.Dto 
{
    public class OrderCreateDto
    {
        [Required]
        public DateTime deliveryTime { get; set; }  

        [Required]
        public string address { get; set; }
    }
}