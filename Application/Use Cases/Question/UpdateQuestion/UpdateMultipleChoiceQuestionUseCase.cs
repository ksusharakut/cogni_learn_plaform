using Application.Exceptions;
using Application.Use_Cases.Question.DTOs;
using AutoMapper;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Question.UpdateQuestion
{
    public class UpdateMultipleChoiceQuestionUseCase : IUpdateMultipleChoiceQuestionUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateMultipleChoiceQuestionDTO> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UpdateMultipleChoiceQuestionUseCase(
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

        public async Task ExecuteAsync(int questionId, CreateMultipleChoiceQuestionDTO request, CancellationToken cancellationToken)
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

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null || !user.Roles.Any(r => r.RoleName == "Author"))
            {
                throw new AuthorizationException("Пользователь не имеет роль 'Author'.");
            }

            var question = await _unitOfWork.QuestionRepository.GetByIdWithLessonAsync(questionId, cancellationToken);
            if (question == null)
            {
                throw new NotFoundException($"Вопрос с идентификатором {questionId} не найден.");
            }

            if (question.Lesson.Chapter.Course.UserId != userId)
            {
                throw new AuthorizationException("Вы можете обновлять вопросы только в своих курсах.");
            }

            if (question.QuestionType != QuestionType.MultipleChoice)
            {
                throw new FluentValidation.ValidationException(new[]
                {
                    new ValidationFailure("QuestionType", "Этот use case предназначен только для вопросов с множественным выбором.")
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

            _mapper.Map(request, question);
            question.AnswerOptions = _mapper.Map<List<Domain.Entities.AnswerOption>>(request.AnswerOptions);
            question.CorrectAnswer = null; 

            _unitOfWork.QuestionRepository.Update(question);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            foreach (var option in question.AnswerOptions)
            {
                option.QuestionId = questionId;
            }
        }
    }
}
