using Application.Use_Cases.Role.DTOs;
using Domain.Entities;
using AutoMapper;

namespace Infrastructure.AutoMapper
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleDTO, Role>();
        }
    }
}
