using Application.Use_Cases.Auth.DTOs;
using Application.Use_Cases.Role.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.AutoMapper
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<UserRegisterDTO, User>();
        }
    }
}
