
using AutoMapper;
using AssociationManagement.Core.Entities;
using Softylines.Compta.Application.Dtos;
using Softylines.Compta.Application.Dtos.User;

namespace Softylines.Compta.Application.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>();
            CreateMap<ApplicationUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUserReadDto>();
            CreateMap<ApplicationUserReadDto, ApplicationUser>();
            CreateMap<UpdateApplicationUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, UpdateApplicationUserDto>();
        }
    }
}
