using System.ComponentModel.DataAnnotations;

namespace KeyBooking_backend.Dto
{
    public class KeyCreateDto
    {
        [Required]
        public int Number { get; set; }
    }
}
