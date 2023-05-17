using BastilleIUserLibrary.Domain.Enums;
using BastilleUserLibrary.Infrastructure;
using BastilleUserLibrary.Infrastructure.Repositories;
using BastilleUserService.Core.DTOs;
using BastilleUserService.Core.DTOs.Request;
using BastilleUserService.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BastilleUserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("signUpBuyer")]
        public async Task<IActionResult> SignUpBuyer([FromBody] RegistrationDTO request)
        {
            var result = await _authService.SignUp(request,UserRole.Buyer);
            return Ok(result);
        }
        [HttpPost]
        [Route("signUpSeller")]
        public async Task<IActionResult> SignUpSeller([FromBody] RegistrationDTO request)
        {
            var result = await _authService.SignUp(request, UserRole.Seller);
            return Ok(result);
        }

        [HttpPost]
        [Route("logIn")]
        public async Task<IActionResult> LogIn([FromBody] LoginDTO request)
        {
            var result = await _authService.Login(request);
            return Ok(result);
        }


        [HttpPatch]
        [Route("Address")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddAddress([FromBody] AddressDTO model)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(i => i.Type == "Id").Value;
            if (userId== null)
            {
                return BadRequest();
            }
            var result = await _authService.AddAddress(model, userId);
            return StatusCode(result.StatusCode, result);
        }


    }
}
