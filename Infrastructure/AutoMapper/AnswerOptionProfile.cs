using Application.Use_Cases.AnswerOption.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.AutoMapper
{
    public class AnswerOptionProfile : Profile
    {
        public AnswerOptionProfile()
        {
            CreateMap<CreateAnswerOptionDTO, AnswerOption>();
            CreateMap<AnswerOption, AnswerOptionDTO>();
        }
    }
}
