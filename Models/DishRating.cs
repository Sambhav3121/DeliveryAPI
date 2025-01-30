using System;
using System.ComponentModel.DataAnnotations;

namespace sambackend.Models
{
    public class DishRating
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid DishId { get; set; } 

        [Required]
        public string UserId { get; set; } 

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        // Navigation property
        public Dish Dish { get; set; }
    }
}