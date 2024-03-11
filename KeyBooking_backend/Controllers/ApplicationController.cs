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
    [Route("api")]
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
                    ex.Message == "You have already submitted a request for this time!" ||
                    ex.Message == "Key mentioned in application does not exist!" ||
                    ex.Message == "Period mentioned in application does not exist!")
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

        [Authorize(Roles = "Student,Teacher")]
        [HttpGet]
        [Route("myApplications")]
        public async Task<ActionResult<ApplicationsListDto>> GetMyApplicationsInfo()
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
                return await _applicationService.GetMyApplicationsInfo(userEmail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong");
            }
        }

        [Authorize(Roles = "Student,Teacher")]
        [HttpGet]
        [Route("myApplications/{id}")]
        public async Task<ActionResult<ApplicationInfoDto>> GetMyApplicationInfo(string id)
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
                return await _applicationService.GetMyApplicationInfo(id, userEmail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong");
            }
        }


        [Authorize(Roles = "Deanery")]
        [HttpPost]
        [Route("application/{id}/approve")]
        public async Task<IActionResult> ApproveApplication(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _applicationService.ApproveApplication(id);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message == "This application does not exist!" ||
                    ex.Message == "You can approve only new applications!")
                {
                    return StatusCode(400, ex.Message);
                }
                return StatusCode(500, "Something went wrong");
            }
        }

        [Authorize(Roles = "Deanery")]
        [HttpPost]
        [Route("application/{id}/reject")]
        public async Task<IActionResult> RejectApplication(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _applicationService.RejectApplication(id);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message == "This application does not exist!" ||
                    ex.Message == "You can't reject already rejected applications!" ||
                    ex.Message == "You cannot reject the application for which the key was issued!")
                {
                    return StatusCode(400, ex.Message);
                }
                return StatusCode(500, "Something went wrong");
            }
        }

        [Authorize(Roles = "Student,Teacher")]
        [HttpDelete]
        [Route("application/{id}/recall")]
        public async Task<IActionResult> RecallApplication(string id)
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
                await _applicationService.RecallApplication(id, userEmail);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message == "This application does not exist!" ||
                    
                    ex.Message == "You cannot recall the application for which the key was issued!")
                {
                    return StatusCode(400, ex.Message);
                }
                else if (ex.Message == "The application you are trying to withdraw does not belong to you!")
                {
                    return StatusCode(403, ex.Message);
                }
                return StatusCode(500, ex.Message); //"Something went wrong");
            }
        }
    }
}
