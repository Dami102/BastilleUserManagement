using BastilleUserService.Core.DTOs;
using BastilleUserService.Core.DTOs.Response;
using Microsoft.AspNetCore.Http;

namespace BastilleUserService.Core.Interfaces
{
    public interface IUserProfileService
    {
        Task<APIResponse<GetUserDTO>> GetUserByEmail(string email);
        Task<APIResponse<UserProfileDTO>> GetUserProfile(string id);
        Task<APIResponse<string>> UploadImageAsync(string id, IFormFile file);
    }
}