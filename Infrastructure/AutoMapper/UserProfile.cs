using Application.Use_Cases.Auth.DTOs;
using Application.Use_Cases.User;
using Application.Use_Cases.User.DTOs;
using Application.Use_Cases.User.UpdateUserProfile;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.RoleName)));

            CreateMap<UpdateUserProfileDTO, User>();

            CreateMap<User, UserProfileDTO>();

            CreateMap<UserRegisterDTO, User>();
        }
    }
}
