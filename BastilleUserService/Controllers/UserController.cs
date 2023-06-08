using BastilleUserService.Core.DTOs.Request;
using BastilleUserService.Core.Interfaces;
using BastilleUserService.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BastilleUserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IAuthService _authService;
        public UserController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
            _authService = _authService;
        }
        // GET: api/<UserController>
        //Get User Profile
        [HttpGet]
        [Route("UserProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = HttpContext.User.Claims.First(user => user.Type == "Id").Value;
            if(userId == null)
            {
                return BadRequest();
            }
            var result = await _userProfileService.GetUserProfile(userId);
            return StatusCode(result.StatusCode,result);
        }
        [HttpGet("get-user/{Email}")]
        public async Task<IActionResult> GetUserByID(string email)
        {
            var response = await _userProfileService.GetUserByEmail(email);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [Route("AddAmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminOnly")]
        public async Task<IActionResult> AddAdmin([FromBody] RegistrationDTO model)
        {
            var result = await _authService.AddAdmin(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(i => i.Type == "Id").Value;
            var result = await _userProfileService.UploadImageAsync(userId, file);
            return StatusCode(result.StatusCode, result);
        }


    }
}
