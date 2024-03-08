using System.ComponentModel.DataAnnotations;

namespace KeyBooking_backend.Models
{
    public class Period
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public TimeOnly TimeStart { get; set; }
        [Required]
        public TimeOnly TimeEnd { get; set;}
    }
}
