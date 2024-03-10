using System.ComponentModel.DataAnnotations;

namespace KeyBooking_backend.Dto
{
    public class UserEmailDto
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
