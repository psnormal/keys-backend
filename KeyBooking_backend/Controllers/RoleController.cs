using KeyBooking_backend.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace KeyBooking_backend.Controllers
{
    
    [Route("api/role")]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        private const string userDontExistMessage = "User with provided email don't exist!";

        public RoleController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin,Deanery")]
        [HttpPost("add/student")]
        public async Task<IActionResult> AddStudentRole([FromBody] UserEmailDto roleInfo)
        {
            var user = await _userManager.FindByEmailAsync(roleInfo.Email);
            if (user == null)
            {
                return BadRequest(userDontExistMessage);
            }

            await _userManager.AddToRoleAsync(user, "Student");

            return Ok();
        }

        [Authorize(Roles = "Admin,Deanery")]
        [HttpPost("add/teacher")]
        public async Task<IActionResult> AddTeacherRole([FromBody] UserEmailDto roleInfo)
        {
            var user = await _userManager.FindByEmailAsync(roleInfo.Email);
            if (user == null)
            {
                return BadRequest(userDontExistMessage);
            }

            await _userManager.AddToRoleAsync(user, "Teacher");

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add/deanery")]
        public async Task<IActionResult> AddDeaneryRole([FromBody] UserEmailDto roleInfo)
        {
            var user = await _userManager.FindByEmailAsync(roleInfo.Email);
            if (user == null)
            {
                return BadRequest(userDontExistMessage);
            }

            await _userManager.AddToRoleAsync(user, "Deanery");

            return Ok();
        }

        [Authorize(Roles = "Admin,Deanery")]
        [HttpDelete("remove/student")]
        public async Task<IActionResult> RemoveStudentRole([FromBody] UserEmailDto roleInfo)
        {
            var user = await _userManager.FindByEmailAsync(roleInfo.Email);
            if (user == null)
            {
                return BadRequest(userDontExistMessage);
            }

            await _userManager.RemoveFromRoleAsync(user, "Student");

            return Ok();
        }

        [Authorize(Roles = "Admin,Deanery")]
        [HttpDelete("remove/teacher")]
        public async Task<IActionResult> RemoveTeacherRole([FromBody] UserEmailDto roleInfo)
        {
            var user = await _userManager.FindByEmailAsync(roleInfo.Email);
            if (user == null)
            {
                return BadRequest(userDontExistMessage);
            }

            await _userManager.RemoveFromRoleAsync(user, "Teacher");

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("remove/deanery")]
        public async Task<IActionResult> RemoveDeaneryRole([FromBody] UserEmailDto roleInfo)
        {
            var user = await _userManager.FindByEmailAsync(roleInfo.Email);
            if (user == null)
            {
                return BadRequest(userDontExistMessage);
            }

            await _userManager.RemoveFromRoleAsync(user, "Deanery");

            return Ok();
        }




    }
}
