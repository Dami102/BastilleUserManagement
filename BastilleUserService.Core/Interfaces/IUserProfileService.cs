using BastilleUserService.Core.DTOs;

namespace BastilleUserService.Core.Interfaces
{
    public interface IUserProfileService
    {
        Task<ResponseDTO<GetUserDTO>> GetUserById(string id);
        Task<ResponseDTO<UserProfileDTO>> GetUserProfile(string id);
    }
}