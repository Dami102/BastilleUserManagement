using AutoMapper;
using BastilleIUserLibrary.Domain.Enums;
using BastilleIUserLibrary.Domain.Model;
using BastilleUserService.Core.DTOs;
using BastilleUserService.Core.DTOs.Request;
using BastilleUserService.Core.DTOs.Response;
using BastilleUserService.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BastilleUserService.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        public AuthService(IServiceProvider serviceProvider, IMapper mapper, ILogger<AuthService> logger, ITokenService tokenService)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<APIResponse<string>> SignUp(RegistrationDTO request, UserRole appUserRole)
        {
            _logger.LogInformation("SignUp Started");

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                _logger.LogInformation("Error, user already exist");
                return APIResponse<string>.Fail("Email Already Exist", (int)HttpStatusCode.BadRequest);
            }

            _logger.LogInformation("Mapping User");

            var mappedUser = _mapper.Map<User>(request);
            mappedUser.UserName = request.Email;
            mappedUser.IsActive= true;
            _logger.LogInformation("Creating User");

            var createUser = await _userManager.CreateAsync(mappedUser, request.Password);

            if (!createUser.Succeeded)
            {
                _logger.LogInformation("Error, when creating user");
                return APIResponse<string>.Fail($"{createUser.Errors}", (int)HttpStatusCode.InternalServerError);
            }

            _logger.LogInformation("Adding User Roles");

            IdentityResult addUserRole;

            if(appUserRole == UserRole.Buyer || appUserRole == UserRole.Seller)
            {
                addUserRole = await _userManager.AddToRoleAsync(mappedUser, appUserRole.ToString());
            }
            else
            {
                _logger.LogInformation("Error, Trying to add invalid role");
                return APIResponse<string>.Fail($"Invalid user type", (int)HttpStatusCode.BadRequest);
            }
            if (!addUserRole.Succeeded)
            {
                _logger.LogInformation("Error, when adding user Role");
                return APIResponse<string>.Fail($"{addUserRole.Errors}", (int)HttpStatusCode.InternalServerError);
            }

            _logger.LogInformation("User Created");

            return APIResponse<string>.Success("You have scuussefully signedUp", $"{request.Email}", (int)HttpStatusCode.Created);

        }

        public async Task<APIResponse<LoginResponseDTO>> Login(LoginDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user == null)
            {
                return APIResponse<LoginResponseDTO>.Fail("User does not exist", (int)HttpStatusCode.BadRequest);
            }

            if(!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return APIResponse<LoginResponseDTO>.Fail("Inavlid user credential", (int)HttpStatusCode.BadRequest);
            }

            /*if (!user.IsActive)
            {
                return APIResponse<LoginResponseDTO>.Fail("User's account is Deactivated", (int)HttpStatusCode.BadRequest);

            }*/

            var refreshToken = _tokenService.GenerateRefreshToken();
          
            var response = new LoginResponseDTO()
            {
                Id = user.Id,
                Token = await _tokenService.GenerateToken(user),
                RefreshToken = refreshToken
            };

            _logger.LogInformation("User successfully logged in");
            return APIResponse<LoginResponseDTO>.Success("Login successful", response);
        }

        public async Task<ResponseDTO<bool>> AddAddress(AddressDTO address, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ResponseDTO<bool>.Fail("User not found", (int)HttpStatusCode.BadRequest);
            }

            user.Address = $"StreetNumber: {address.StreetNumber}, City: {address.City}, State: {address.State},  Country: {address.Country}";
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return ResponseDTO<bool>.Success("User address Added", true, (int)HttpStatusCode.Created);
            else
                return ResponseDTO<bool>.Fail("Service not available");
        }
    }
}
