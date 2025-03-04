using Application.Exceptions;
using Application.Use_Cases.Question.DTOs;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Question.UpdateQuestion
{
    public class UpdateQuestionUseCase : IUpdateQuestionUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateQuestionDTO> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UpdateQuestionUseCase(
            IUnitOfWork unitOfWork,
            IValidator<CreateQuestionDTO> validator,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(int questionId, CreateQuestionDTO request, CancellationToken cancellationToken)
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

            _mapper.Map(request, question);

            _unitOfWork.QuestionRepository.Update(question);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
