using BastilleIUserLibrary.Domain.Model;
using BastilleUserService.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BastilleUserService.Core.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _manager;

        public TokenService(IConfiguration configuration, UserManager<User> manager)
        {
            _configuration = configuration;
            _manager = manager;
        }
        public async Task<string> GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:Token"]));
            var claims = await GetAllValidClaims(user);
        
            
            var jwtConfig = _configuration.GetSection("Jwt");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtConfig.GetSection("lifetime").Value)),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512Signature),
                Issuer = _configuration["jwt:Issuer"],
                Audience = _configuration["jwt:Audience"]
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
        
            return jwtToken;
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
        private async Task<List<Claim>> GetAllValidClaims(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var userClaims = await _manager.GetClaimsAsync(user);
            authClaims.AddRange(userClaims);

            var userRoles = await _manager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            return authClaims;
        }
    }
}
