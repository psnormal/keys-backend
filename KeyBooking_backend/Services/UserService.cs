using KeyBooking_backend.Dto;
using System.ComponentModel.DataAnnotations;

namespace KeyBooking_backend.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public UserInfoDto GetUserInfo(string id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new ValidationException("This user does not exist");
            }

            UserInfoDto result = new UserInfoDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                UserEmail = user.Email
            };
            return result;
        }

        public UsersListDto GetUsersInfo()
        {
            var users = _context.Users.ToList();
            var result1 = new List<UserInfoDto>();
            foreach (var user in users)
            {   
                var user1 = new UserInfoDto
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    UserEmail = user.Email
                };
                result1.Add(user1);
            }
            var result = new UsersListDto(result1);
            return result;
        }





    }
}
