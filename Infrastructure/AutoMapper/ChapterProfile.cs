using Application.Use_Cases.Chapter.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.AutoMapper
{
    public class ChapterProfile : Profile
    {
        public ChapterProfile()
        {
            CreateMap<CreateChapterDTO, Chapter>();

            CreateMap<Chapter, ChapterDetailsDTO>()
                .ForMember(dest => dest.Lessons, opt => opt.MapFrom(src => src.Lessons.OrderBy(l => l.OrderIndex)));
        }
    }
}
