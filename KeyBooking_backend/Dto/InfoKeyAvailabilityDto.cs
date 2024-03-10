using KeyBooking_backend.Models;
using System.ComponentModel.DataAnnotations;

namespace KeyBooking_backend.Dto
{
    public class InfoKeyAvailabilityDto
    {
        [Required]
        public List<int> periodsNumbers { get; set; }

        public InfoKeyAvailabilityDto(List<int> periods)
        {
            periodsNumbers = periods;
        }
    }
}
