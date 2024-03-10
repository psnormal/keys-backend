using KeyBooking_backend.Dto;
using KeyBooking_backend.Models;
using KeyBooking_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace KeyBooking_backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class KeyController : ControllerBase
    {
        private IKeyService _keyService;

        public KeyController(IKeyService keyService)
        {
            _keyService = keyService;
        }

        [Authorize(Roles = "Admin,Deanery")]
        [HttpPost]
        [Route("key/create")]
        public async Task<ActionResult<InfoKeyDto>> CreateKey(KeyCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _keyService.CreateKey(model);
                return GetKeyInfo(model.Number);
            }
            catch (Exception ex)
            {
                if (ex.Message == "This key already exists")
                {
                    return StatusCode(400, ex.Message);
                }
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpGet]
        [Route("key/{number}")]
        public ActionResult<InfoKeyDto> GetKeyInfo(int number)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                return _keyService.GetKeyInfo(number);
            }
            catch (Exception ex)
            {
                if (ex.Message == "This key does not exist")
                {
                    return StatusCode(400, ex.Message);
                }
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpGet]
        [Route("keys")]
        public ActionResult<InfoKeysDto> GetKeysInfo()
        {
            try
            {
                return _keyService.GetKeysInfo();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpDelete]
        [Route("key/{number}/delete")]
        public async Task<ActionResult> DeleteKey(int number)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _keyService.DeleteKey(number);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message == "This key does not exist")
                {
                    return StatusCode(400, ex.Message);
                }
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpGet]
        [Route("{id}/keys")]
        public ActionResult<InfoKeysDto> GetKeysUser(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                return _keyService.GetKeysUser(id);
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

        [HttpPut]
        [Route("key/{number}/transfer")]
        public async Task<ActionResult<InfoKeyDto>> TransferKey(int number, Key model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _keyService.TransferKey(model);
                return _keyService.GetKeyInfo(number);
            }
            catch (Exception ex)
            {
                if (ex.Message == "This user does not exist")
                {
                    return StatusCode(400, ex.Message);
                }
                if (ex.Message == "This key does not exist")
                {
                    return StatusCode(400, ex.Message);
                }
                if (ex.Message == "This key alredy in deanery")
                {
                    return StatusCode(400, ex.Message);
                }
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
