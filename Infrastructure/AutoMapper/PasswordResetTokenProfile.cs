using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.AutoMapper
{
    public class PasswordResetTokenProfile : Profile
    {
        public PasswordResetTokenProfile()
        {
            CreateMap<PasswordResetToken, PasswordResetToken>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => TokenStatus.Active));
        }
    }
}
