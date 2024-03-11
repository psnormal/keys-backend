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

            var userRoles = _context.UserRoles.Where(x => x.UserId == user.Id).ToList();
            List<string> returnedRoles = new List<string>();

            foreach (var role in userRoles)
            {
                var nameRole = _context.Roles.FirstOrDefault(x => x.Id == role.RoleId).Name;
                returnedRoles.Add(nameRole);
            }


            UserInfoDto result = new UserInfoDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                UserEmail = user.Email,
                roles = returnedRoles
            };
            return result;
        }

        public UsersListDto GetUsersInfo()
        {
            var users = _context.Users.ToList();
            var result1 = new List<UserInfoDto>();
            foreach (var user in users)
            {
                var userRoles = _context.UserRoles.Where(x => x.UserId == user.Id).ToList();
                List<string> returnedRoles = new List<string>();

                foreach (var role in userRoles)
                {
                    var nameRole = _context.Roles.FirstOrDefault(x => x.Id == role.RoleId).Name;
                    returnedRoles.Add(nameRole);
                }

                var user1 = new UserInfoDto
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    UserEmail = user.Email,
                    roles = returnedRoles
                };
                result1.Add(user1);
            }
            var result = new UsersListDto(result1);
            return result;
        }
    }
}
