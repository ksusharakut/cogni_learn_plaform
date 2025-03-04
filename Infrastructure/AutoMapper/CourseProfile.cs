using Application.Use_Cases.Course.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.AutoMapper
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDTO>()
                .ForMember(dest => dest.AuthorEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(c => c.CategoryName)));

            CreateMap<CreateCourseDTO, Course>();

            CreateMap<Course, CourseDetailsDTO>()
                .ForMember(dest => dest.AuthorEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(c => c.CategoryName)))
                .ForMember(dest => dest.Chapters, opt => opt.MapFrom(src => src.Chapters.OrderBy(c => c.OrderIndex)));
        }
    }
}
