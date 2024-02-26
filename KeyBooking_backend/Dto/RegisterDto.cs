using System.ComponentModel.DataAnnotations;

namespace KeyBooking_backend.Dto
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
