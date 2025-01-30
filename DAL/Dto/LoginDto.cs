using System.ComponentModel.DataAnnotations;

namespace sambackend.Models
{
    public class LoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format. Please use a valid email like user@example.com.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
     }
}