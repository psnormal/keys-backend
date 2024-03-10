using KeyBooking_backend.Dto;
using KeyBooking_backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace KeyBooking_backend.Controllers
{
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("user/{id}")]
        public ActionResult<UserInfoDto> GetKeyInfo(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                return _userService.GetUserInfo(id);
            }
            catch (Exception ex)
            {
                if (ex.Message == "This user does not exist")
                {
                    return StatusCode(400, ex.Message);
                }
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpGet]
        [Route("users")]
        public ActionResult<UsersListDto> GetUsersInfo()
        {
            try
            {
                return _userService.GetUsersInfo();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
