using System.ComponentModel.DataAnnotations;

namespace KeyBooking_backend.Dto
{
    public class ApplicationsListDto
    {
        [Required]
        public List<ApplicationInfoDto> applications { get; set; }

        public ApplicationsListDto(List<ApplicationInfoDto> applications)
        {
            this.applications = applications;
        }
    }
}
