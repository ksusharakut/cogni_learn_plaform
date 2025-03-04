using Application.Use_Cases.Lesson.DTOs;
using Application.Use_Cases.Lesson;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.AutoMapper
{
    public class LessonProfile : Profile
    {
        public LessonProfile()
        {
            CreateMap<CreateLessonDTO, Lesson>();

            CreateMap<Lesson, LessonDTO>();

            CreateMap<Lesson, LessonDetailsDTO>();
        }
    }
}
