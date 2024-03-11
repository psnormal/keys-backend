using KeyBooking_backend.Dto;
using KeyBooking_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Claims;

namespace KeyBooking_backend.Controllers
{
    [Route("api/application")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContext;
        private IApplicationService _applicationService;

        public ApplicationController(IHttpContextAccessor httpContext, IApplicationService applicationService)
        {
            _httpContext = httpContext;
            _applicationService = applicationService;
        }



        [Authorize(Roles = "Student,Teacher")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationDto applicationInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var currentUser = _httpContext.HttpContext.User;
            string userEmail = "";
            foreach (var i in currentUser.Claims)
            {
                if (i.Type == ClaimTypes.Email)
                {
                    userEmail = i.Value;
                }
            }

            if (userEmail == "")
            {
                return BadRequest();
            }

            try
            {
                await _applicationService.CreateApplication(applicationInfo, userEmail);
                return Ok();
            }
            catch
            (Exception ex)
            {
                if (ex.Message == "This time is already taken!" || 
                    ex.Message == "You have already submitted a request for this time!")
                {
                    return StatusCode(400, ex.Message);
                }

                return StatusCode(500, "Something went wrong");
            }
        }

        [Authorize(Roles = "Deanery")]
        [HttpGet]
        [Route("application/{id}")]
        public ActionResult<ApplicationInfoDto> GetApplicationInfo(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                return _applicationService.GetApplicationInfo(id);
            }
            catch (Exception ex)
            {
                if (ex.Message == "This application does not exist!")
                {
                    return StatusCode(400, ex.Message);
                }
                return StatusCode(500, "Something went wrong");
            }
        }

        [Authorize(Roles = "Deanery")]
        [HttpGet]
        [Route("applications")]
        public ActionResult<ApplicationsListDto> GetApplicationsInfo()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                return _applicationService.GetApplicationsInfo();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong");
            }
        }


    }
}
