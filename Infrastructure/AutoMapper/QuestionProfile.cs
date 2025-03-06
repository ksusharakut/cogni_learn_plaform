using Application.Use_Cases.Question.DTOs;
using Application.Use_Cases.AnswerOption.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.AutoMapper
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<CreateMultipleChoiceQuestionDTO, Question>();
            CreateMap<CreateOpenEndedQuestionDTO, Question>();
            CreateMap<CreateAnswerOptionDTO, AnswerOption>();
            CreateMap<Question, QuestionDTO>();
        }
    }
}
