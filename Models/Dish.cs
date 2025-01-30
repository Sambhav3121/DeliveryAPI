using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sambackend.Models
{
    public class Dish
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        public string Image { get; set; }

        public bool Vegetarian { get; set; } 

        public double Rating { get; set; } 

        public DishCategory Category { get; set; } 

        // Navigation property for ratings
        public List<DishRating> Ratings { get; set; } = new List<DishRating>();
    }
}