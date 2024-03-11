using KeyBooking_backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KeyBooking_backend.Dto
{
    public class ApplicationInfoDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        [ForeignKey("Period")]
        public int PeriodId { get; set; }
        [Required]
        [ForeignKey("Key")]
        public int KeyId { get; set; }
        [Required]
        public Guid Owner { get; set; }
        [Required]
        public ApplicationState State { get; set; }



    }
}
