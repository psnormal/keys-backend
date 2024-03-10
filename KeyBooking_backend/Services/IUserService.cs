using KeyBooking_backend.Dto;

namespace KeyBooking_backend.Services
{
    public interface IUserService
    {
        UserInfoDto GetUserInfo(string id);
        UsersListDto GetUsersInfo();
    }
}
