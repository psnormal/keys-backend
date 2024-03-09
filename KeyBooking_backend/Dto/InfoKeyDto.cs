using KeyBooking_backend.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace KeyBooking_backend.Dto
{
    public class InfoKeyDto
    {
        [Required]
        public int Number { get; set; }
        [Required]
        public KeyState State { get; set; }
        public Guid UserId { get; set; }

        public InfoKeyDto(Key model)
        {
            Number = model.Number;
            State = model.State;
            UserId = model.UserId;
        }
    }
}
