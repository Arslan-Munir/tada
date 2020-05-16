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
    }
}