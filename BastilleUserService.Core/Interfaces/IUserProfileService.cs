using BastilleUserService.Core.DTOs;
using BastilleUserService.Core.DTOs.Response;

namespace BastilleUserService.Core.Interfaces
{
    public interface IUserProfileService
    {
        Task<APIResponse<GetUserDTO>> GetUserById(string id);
        Task<APIResponse<UserProfileDTO>> GetUserProfile(string id);
    }
}