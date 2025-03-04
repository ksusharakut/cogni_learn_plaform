using Application.Exceptions;
using Application.Use_Cases.AnswerOption.DTOs;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.AnswerOption.CreateAnswerOption
{
    public class CreateAnswerOptionUseCase : ICreateAnswerOptionUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateAnswerOptionDTO> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CreateAnswerOptionUseCase(
            IUnitOfWork unitOfWork,
            IValidator<CreateAnswerOptionDTO> validator,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<int> ExecuteAsync(int questionId, CreateAnswerOptionDTO request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new AuthenticationFailedException("Невозможно идентифицировать текущего пользователя.");
            }

            var question = await _unitOfWork.QuestionRepository.GetByIdWithLessonAsync(questionId, cancellationToken);
            if (question == null)
            {
                throw new NotFoundException($"Вопрос с идентификатором {questionId} не найден.");
            }

            if (question.Lesson.Chapter.Course.UserId != userId)
            {
                throw new AuthorizationException("Вы можете добавлять варианты ответа только в свои курсы.");
            }

            var answerOption = _mapper.Map<Domain.Entities.AnswerOption>(request);
            answerOption.QuestionId = questionId;

            await _unitOfWork.AnswerOptionRepository.AddAsync(answerOption, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return answerOption.AnswerOptionId;
        }
    }
}
