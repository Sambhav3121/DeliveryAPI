using System;
using System.ComponentModel.DataAnnotations;

namespace sambackend.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [RegularExpression("^(Male|Female|Other)$")]
        public string Gender { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
