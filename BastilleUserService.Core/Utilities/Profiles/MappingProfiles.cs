using AutoMapper;
using BastilleIUserLibrary.Domain.Model;
using BastilleUserService.Core.DTOs;
using BastilleUserService.Core.DTOs.Request;

namespace BastilleUserService.Core.Utilities.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<RegistrationDTO, User>().ReverseMap();
            CreateMap<UserProfileDTO, User>().ReverseMap();
            CreateMap<GetUserDTO, User>().ReverseMap();
        }
    }
}
