using BastilleIUserLibrary.Domain.Enums;
using BastilleIUserLibrary.Domain.Model;
using BastilleUserService.Core.DTOs;
using BastilleUserService.Core.DTOs.Request;
using BastilleUserService.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
            return StatusCode(result.StatusCode,result);
        }
        [HttpPost]
        [Route("signUpSeller")]
        public async Task<IActionResult> SignUpSeller([FromBody] RegistrationDTO request)
        {
            var result = await _authService.SignUp(request, UserRole.Seller);
            return StatusCode(result.StatusCode,result);
        }

        [HttpPost]
        [Route("logIn")]
        public async Task<IActionResult> LogIn([FromBody] LoginDTO request)
        {
            var result = await _authService.Login(request);
            return StatusCode(result.StatusCode, result);
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

        //Cofirm email update

        [HttpPost]
        [Route("ConfirmEmail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ConfirmEmail([FromQuery]ConfirmEmailDTO model)
        {
            var result = await _authService.ConfirmEmail(model);
            return StatusCode(result.StatusCode, result);
        }

       
        [HttpPatch]
        [Route("ChangeUserRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminOnly")]
        public async Task<IActionResult> ChangeRole([FromBody] ChangeUserRoleDTO model)
        {
            if(model.RoleName == "Admin")
            {
                var adminresult = await _authService.ChangeUserRole(model.Email, UserRole.Admin);
                return StatusCode(adminresult.StatusCode, adminresult);
            }
            if (model.RoleName == "Buyer")
            {
                var buyerresult = await _authService.ChangeUserRole(model.Email, UserRole.Buyer);
                return StatusCode(buyerresult.StatusCode, buyerresult);
            }

            var result = await _authService.ChangeUserRole(model.Email, UserRole.Seller);
            return StatusCode(result.StatusCode, result);

        }

        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            return StatusCode(200, "dkdk");
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            /*var userId = HttpContext.User.Claims.FirstOrDefault(i => i.Type == "Id").Value;*/
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           var response = await _authService.Refresh(refreshRequest);
            return StatusCode(response.StatusCode, response);

        }

        [Authorize]
        [HttpPost]
        [Route("LogOut")]
        public async Task<IActionResult> Logout()
        {
            var userEmail = HttpContext.User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Email).Value;
            var response = await _authService.LogOut(userEmail);

            return StatusCode(response.StatusCode, response);   
        }
    }
}
