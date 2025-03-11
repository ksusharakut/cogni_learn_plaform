using Application.Exceptions;
using Application.Use_Cases.AnswerOption.DTOs;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.AnswerOption.UpdateAnswerOption
{
    public class UpdateAnswerOptionUseCase : IUpdateAnswerOptionUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateAnswerOptionDTO> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UpdateAnswerOptionUseCase(
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

        public async Task ExecuteAsync(int answerOptionId, CreateAnswerOptionDTO request, CancellationToken cancellationToken)
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

            var answerOption = await _unitOfWork.AnswerOptionRepository.GetByIdWithQuestionAsync(answerOptionId, cancellationToken);
            if (answerOption == null)
            {
                throw new NotFoundException($"Вариант ответа с идентификатором {answerOptionId} не найден.");
            }

            if (answerOption.Question.Lesson.Chapter.Course.UserId != userId)
            {
                throw new AuthorizationException("Вы можете обновлять варианты ответа только в своих курсах.");
            }

            _mapper.Map(request, answerOption);

            _unitOfWork.AnswerOptionRepository.Update(answerOption);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
