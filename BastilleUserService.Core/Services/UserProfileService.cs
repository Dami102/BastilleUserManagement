using AutoMapper;
using BastilleIUserLibrary.Domain.Model;
using BastilleUserService.Core.DTOs;
using BastilleUserService.Core.DTOs.Response;
using BastilleUserService.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BastilleUserService.Core.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserProfileService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager=userManager;
            _mapper=mapper;
        }

        public async Task<APIResponse<UserProfileDTO>> GetUserProfile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return APIResponse<UserProfileDTO>.Fail("User not found");
            }
            var userDetails = _mapper.Map<UserProfileDTO>(user);
            return APIResponse<UserProfileDTO>.Success("Success", userDetails);
        }

        public async Task<APIResponse<GetUserDTO>> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return APIResponse<GetUserDTO>.Fail("User not found");
            }
            var userinfo = _mapper.Map<GetUserDTO>(user);
            return APIResponse<GetUserDTO>.Success("User details", userinfo);
        }
    }
}
