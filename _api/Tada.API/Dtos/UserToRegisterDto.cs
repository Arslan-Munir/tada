using System;
using System.ComponentModel.DataAnnotations;

namespace Tada.API.Dtos
{
    public class UserToRegisterDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "Password is required.")] 
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Password must be b/w 8 and 4 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Known as is required.")]
        public string KnownAs { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }

        public DateTime Created = DateTime.Now;
        public DateTime LastActive = DateTime.Now;
    }
}