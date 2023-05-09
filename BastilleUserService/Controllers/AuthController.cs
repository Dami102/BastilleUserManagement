using BastilleUserLibrary.Infrastructure;
using BastilleUserLibrary.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BastilleUserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        ApplicationDbContext _context;
       /* public AuthController()
        {
        
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register()
        {
            
            UnitOfWork unit = new (_context);
            var result = 
        }

        
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login()
        {
         
        }*/

   
    }
}
