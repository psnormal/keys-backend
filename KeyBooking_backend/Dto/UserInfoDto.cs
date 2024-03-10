using System.ComponentModel.DataAnnotations;

namespace KeyBooking_backend.Dto
{
    public class UserInfoDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserEmail { get; set; }
    }
}
