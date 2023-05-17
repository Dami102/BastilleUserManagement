using BastilleUserService.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BastilleUserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        public UserController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
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
        [HttpGet("get-user/{Id}")]
        public async Task<IActionResult> GetUserByID(string Id)
        {
            var response = await _userProfileService.GetUserById(Id);
            return StatusCode(response.StatusCode, response);
        }

    }
}
