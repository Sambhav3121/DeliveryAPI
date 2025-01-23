using System.ComponentModel.DataAnnotations;

namespace sambackend{
    public class Dish
    {
        public Guid Id { get; set; }
        [Required]
        public string Name  { get; set; }
        public string Description {get; set;}
        [Required]
        public double Price { get; set; }
        public string Image { get; set; }
        public Boolean Vegetarian {get; set;}
        public double Rating{get; set; }
        
    }
}