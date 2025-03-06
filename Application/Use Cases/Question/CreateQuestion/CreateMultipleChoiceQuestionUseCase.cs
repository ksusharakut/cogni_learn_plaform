using Application.Exceptions;
using Application.Use_Cases.Question.DTOs;
using AutoMapper;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Question.CreateQuestion
{
    public class CreateMultipleChoiceQuestionUseCase : ICreateMultipleChoiceQuestionUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateMultipleChoiceQuestionDTO> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CreateMultipleChoiceQuestionUseCase(
            IUnitOfWork unitOfWork,
            IValidator<CreateMultipleChoiceQuestionDTO> validator,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<int> ExecuteAsync(int lessonId, CreateMultipleChoiceQuestionDTO request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new AuthenticationFailedException("Невозможно идентифицировать текущего пользователя.");
            }

            var lesson = await _unitOfWork.LessonRepository.GetByIdWithChapterAsync(lessonId, cancellationToken);
            if (lesson == null)
            {
                throw new NotFoundException($"Урок с идентификатором {lessonId} не найден.");
            }

            if (lesson.Chapter.Course.UserId != userId)
            {
                throw new AuthorizationException("Вы можете добавлять вопросы только в свои курсы.");
            }

            if (lesson.LessonType != LessonType.Test)
            {
                throw new FluentValidation.ValidationException(new[]
                {
                    new ValidationFailure("LessonType", "Вопросы можно добавлять только к тестовым урокам.")
                });
            }

            if (request.AnswerOptions == null || !request.AnswerOptions.Any())
            {
                throw new FluentValidation.ValidationException(new[]
                {
                    new ValidationFailure("AnswerOptions", "Для вопросов с множественным выбором необходимо указать варианты ответа.")
                });
            }
            if (!request.AnswerOptions.Any(a => a.IsCorrect))
            {
                throw new FluentValidation.ValidationException(new[]
                {
                    new ValidationFailure("AnswerOptions", "Должен быть хотя бы один правильный вариант ответа.")
                });
            }

            var maxOrderIndex = await _unitOfWork.QuestionRepository.GetMaxOrderIndexAsync(lessonId, cancellationToken);
            var newOrderIndex = maxOrderIndex + 1;

            var question = _mapper.Map<Domain.Entities.Question>(request);
            question.LessonId = lessonId;
            question.OrderIndex = newOrderIndex;
            question.QuestionType = QuestionType.MultipleChoice; 
            question.AnswerOptions = _mapper.Map<List<Domain.Entities.AnswerOption>>(request.AnswerOptions);

            await _unitOfWork.QuestionRepository.AddAsync(question, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            foreach (var option in question.AnswerOptions)
            {
                option.QuestionId = question.QuestionId;
            }

            return question.QuestionId;
        }
    }
}
