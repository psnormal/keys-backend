using System.ComponentModel.DataAnnotations;

namespace KeyBooking_backend.Dto
{
    public class UsersListDto
    {
        [Required]
        public List<UserInfoDto> users { get; set; }

        public UsersListDto(List<UserInfoDto> users)
        {
            this.users = users;
        }
    }
}
