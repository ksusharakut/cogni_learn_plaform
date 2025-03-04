using Application.Use_Cases.Rating.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.AutoMapper
{
    public class RatingProfile : Profile
    {
        public RatingProfile() 
        {
            CreateMap<AddRatingDTO, Rating>();

            CreateMap<Rating, RatingDTO>()
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email));
        }
            
    }
}
