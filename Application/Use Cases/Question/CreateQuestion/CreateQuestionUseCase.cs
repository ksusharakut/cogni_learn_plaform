using Application.Exceptions;
using Application.Use_Cases.Question.DTOs;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Question.CreateQuestion
{
    public class CreateQuestionUseCase : ICreateQuestionUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateQuestionDTO> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CreateQuestionUseCase(
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

        public async Task<int> ExecuteAsync(int lessonId, CreateQuestionDTO request, CancellationToken cancellationToken)
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

            var lesson = await _unitOfWork.LessonRepository.GetByIdWithChapterAsync(lessonId, cancellationToken);
            if (lesson == null)
            {
                throw new NotFoundException($"Урок с идентификатором {lessonId} не найден.");
            }

            if (lesson.Chapter.Course.UserId != userId)
            {
                throw new AuthorizationException("Вы можете добавлять вопросы только в свои курсы.");
            }

            var maxOrderIndex = await _unitOfWork.QuestionRepository.GetMaxOrderIndexAsync(lessonId, cancellationToken);
            var newOrderIndex = maxOrderIndex + 1;

            var question = _mapper.Map<Domain.Entities.Question>(request);
            question.LessonId = lessonId;
            question.OrderIndex = newOrderIndex;

            await _unitOfWork.QuestionRepository.AddAsync(question, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return question.QuestionId;
        }
    }
}
