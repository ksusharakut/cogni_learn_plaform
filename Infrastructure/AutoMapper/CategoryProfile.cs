using Application.Use_Cases.Category.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.AutoMapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDTO>();

            CreateMap<CreateCategoryDTO, Category>()
               .ForMember(dest => dest.Courses, opt => opt.Ignore());
        }
    }
}
