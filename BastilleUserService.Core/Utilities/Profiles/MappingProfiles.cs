using AutoMapper;
using BastilleIUserLibrary.Domain.Model;
using BastilleUserService.Core.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BastilleUserService.Core.Utilities.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<RegistrationDTO, User>().ReverseMap();
        }
    }
}
