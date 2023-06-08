using AutoMapper;
using BastilleIUserLibrary.Domain.Model;
using BastilleUserService.Core.DTOs;
using BastilleUserService.Core.DTOs.Response;
using BastilleUserService.Core.Interfaces;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace BastilleUserService.Core.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ICloudinaryServices _cloudinaryService;

        public UserProfileService(UserManager<User> userManager, IMapper mapper, ICloudinaryServices cloudinaryService)
        {
            _userManager=userManager;
            _mapper=mapper;
            _cloudinaryService=cloudinaryService;
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

        public async Task<APIResponse<GetUserDTO>> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return APIResponse<GetUserDTO>.Fail("User not found");
            }
            var userinfo = _mapper.Map<GetUserDTO>(user);
            return APIResponse<GetUserDTO>.Success("User details", userinfo);
        }

        public async Task<APIResponse<string>> UploadImageAsync(string id, IFormFile file)
        {
            var user = await _userManager.FindByIdAsync(id);
            UploadResult upload;
            if(user == null)
            {
                return APIResponse<string>.Fail("User is not found");
            }

            if(user.ImageUrl is not null)
            {
                upload = await _cloudinaryService.UpdateByPublicId(file, user.Id);
            }
            else
            {
                upload = await _cloudinaryService.UploadImage(file);
            }
            if(upload is null)
            {
                return APIResponse<string>.Fail("Image is not valid", (int)HttpStatusCode.ServiceUnavailable);
            }

            user.ImageUrl = upload.Url.ToString();
            user.PublicId = upload.PublicId.ToString();
            var response = await _userManager.UpdateAsync(user);
            if (!response.Succeeded)
            {
                await _cloudinaryService.DeleteByPublicId(upload.PublicId);
                return APIResponse<string>.Fail("Image was added to cloudinary but not persisted to the database", (int)HttpStatusCode.InternalServerError);
            }

            return APIResponse<string>.Success("Image uploaded Sucessfully", user.ImageUrl);
        }
    }
}
