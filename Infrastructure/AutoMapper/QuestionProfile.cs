using Application.Use_Cases.Question.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.AutoMapper
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<CreateQuestionDTO, Question>();
            CreateMap<Question, QuestionDTO>();
        }
    }
}
