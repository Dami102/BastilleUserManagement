﻿using AutoMapper;
using BastilleIUserLibrary.Domain.Enums;
using BastilleIUserLibrary.Domain.Model;
using BastilleUserService.Core.DTOs;
using BastilleUserService.Core.DTOs.Request;
using BastilleUserService.Core.DTOs.Response;
using BastilleUserService.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Web;

namespace BastilleUserService.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        public AuthService(IServiceProvider serviceProvider, IMapper mapper, ILogger<AuthService> logger, ITokenService tokenService, IEmailService emailService)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
            _emailService=emailService;
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
            var userFromDb = await _userManager.FindByEmailAsync(mappedUser.Email);
            var token = await  _userManager.GenerateEmailConfirmationTokenAsync(userFromDb);
            var uriBuilder = new UriBuilder("http://localhost:7287/ConfirmEmail");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["token"] = token;
            query["userId"] = userFromDb.Id;
            uriBuilder.Query = query.ToString();
            var urlString = uriBuilder.ToString();

            _emailService.SendEmail(urlString, userFromDb.Email);

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

        public async Task<APIResponse<bool>> AddAddress(AddressDTO address, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return APIResponse<bool>.Fail("User not found", (int)HttpStatusCode.BadRequest);
            }

            user.Address = $"StreetNumber: {address.StreetNumber}, City: {address.City}, State: {address.State},  Country: {address.Country}";
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return APIResponse<bool>.Success("User address Added", true, (int)HttpStatusCode.Created);
            else
                return APIResponse<bool>.Fail("Service not available");
        }

        public async Task<APIResponse<string>> ConfirmEmail(ConfirmEmailDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if(user == null)
            {
                return APIResponse<string>.Fail("User does not Exist");
            }

            var result = await _userManager.ConfirmEmailAsync(user, model.Token);

            if (result.Succeeded)
            {
                return APIResponse<string>.Success("Email Confirmed", "");
            }
            return APIResponse<string>.Fail("Email Confirmation is not successful");
        }

        public async Task<APIResponse<string>> AddAdmin(RegistrationDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user != null)
            {
                return APIResponse<string>.Fail("User does not Exist");
            }

            var mappedUser = _mapper.Map<User>(user);
            mappedUser.Email = model.Email;
            mappedUser.IsActive = true;

            var createdResult = await _userManager.CreateAsync(mappedUser);
            if (!createdResult.Succeeded)
            {
                return APIResponse<string>.Fail("User could not be created", (int)HttpStatusCode.InternalServerError);
            }

            var addAdminRole = await _userManager.AddToRoleAsync(mappedUser, UserRole.Admin.ToString());
            if (!addAdminRole.Succeeded)
            {
                return APIResponse<string>.Fail("Error while adding Role", (int)HttpStatusCode.InternalServerError);
            }

            return APIResponse<string>.Success("Admin Created Successfully", mappedUser.Email);
        }

        public async Task<APIResponse<string>> ChangeUserRole(string email, UserRole userRole)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return APIResponse<string>.Fail("User does not exist");
            }
            var role = await _userManager.GetRolesAsync(user);
            var removeRole = await _userManager.RemoveFromRolesAsync(user, role);

            if (!removeRole.Succeeded)
            {
                return APIResponse<string>.Fail("Roles could not be removed", (int)HttpStatusCode.InternalServerError);
            }
            if (!(userRole == UserRole.Buyer || userRole == UserRole.Admin || userRole == UserRole.Buyer))
            {
                return APIResponse<string>.Fail("Invalid Role");
            }
            var addRole = await _userManager.AddToRoleAsync(user, userRole.ToString());
            if (!addRole.Succeeded)
            {
                return APIResponse<string>.Fail("Changeing Role failed", (int)HttpStatusCode.InternalServerError);
            }
            return APIResponse<string>.Success($"User {user.Email} Successfully changed to {userRole}", user.Email);
        }
    }
}
