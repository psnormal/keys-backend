using System.ComponentModel.DataAnnotations;

namespace KeyBooking_backend.Models
{
    public class Key
    {
        [Required]
        [Key]
        public int Number { get; set; }
        [Required]
        public KeyState State { get; set; }
        public Guid UserId { get; set; }
    }
}
