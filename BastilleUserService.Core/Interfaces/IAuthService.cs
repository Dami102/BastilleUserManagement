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
        Task<APIResponse<bool>> AddAddress(AddressDTO address, string userId);
        Task<APIResponse<string>> ConfirmEmail(ConfirmEmailDTO model);
        Task<APIResponse<string>> AddAdmin(RegistrationDTO model);
        Task<APIResponse<string>> ChangeUserRole(string email, UserRole userRole);
        Task<APIResponse<LoginResponseDTO>> Refresh(RefreshRequest refreshRequest);
        Task<APIResponse<string>> LogOut(string email);
    }
}