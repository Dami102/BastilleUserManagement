using AutoMapper;
using BastilleIUserLibrary.Domain.Model;
using BastilleUserService.Core.DTOs;
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

        public async Task<ResponseDTO<UserProfileDTO>> GetUserProfile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return ResponseDTO<UserProfileDTO>.Fail("User not found");
            }
            var userDetails = _mapper.Map<UserProfileDTO>(user);
            return ResponseDTO<UserProfileDTO>.Success("Success", userDetails);
        }

        public async Task<ResponseDTO<GetUserDTO>> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return ResponseDTO<GetUserDTO>.Fail("User not found");
            }
            var userinfo = _mapper.Map<GetUserDTO>(user);
            return ResponseDTO<GetUserDTO>.Success("User details", userinfo);
        }
    }
}
