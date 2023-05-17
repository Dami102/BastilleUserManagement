using BastilleIUserLibrary.Domain.Enums;
using BastilleUserService.Core.DTOs;
using BastilleUserService.Core.DTOs.Request;
using BastilleUserService.Core.DTOs.Response;

namespace BastilleUserService.Core.Interfaces
{
    public interface IAuthService
    {
        Task<APIResponse<string>> SignUp(RegistrationDTO request, UserRole appUserRole);
        Task<APIResponse<LoginResponseDTO>> Login(LoginDTO request);
        Task<ResponseDTO<bool>> AddAddress(AddressDTO address, string userId);
    }
}