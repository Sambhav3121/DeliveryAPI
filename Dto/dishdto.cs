using System.ComponentModel.DataAnnotations;

namespace sambackend.Dto 
{
    public class DishDto
    {
        public string Id { get; set; } 

        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        public string Image { get; set; }

        public bool IsVegetarian { get; set; } 

        public double Rating { get; set; }

        public DishCategory Category { get; set; }

        public enum DishCategory
        {
            Pizza,
            Pasta,
            Salad,
            
        }
    }
}