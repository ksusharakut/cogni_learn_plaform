using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Lesson.UpdateLesson
{
    public class UpdateLessonUseCase : IUpdateLessonUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateLessonDTO> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UpdateLessonUseCase(
            IUnitOfWork unitOfWork,
            IValidator<CreateLessonDTO> validator,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(int lessonId, CreateLessonDTO request, CancellationToken cancellationToken)
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

            var course = await _unitOfWork.CourseRepository.GetByIdAsync(lesson.Chapter.CourseId, cancellationToken);
            if (course.UserId != userId)
            {
                throw new AuthorizationException("Вы можете обновлять уроки только в своих курсах.");
            }

            _mapper.Map(request, lesson);
            lesson.UpdatedAt = DateTimeOffset.UtcNow;

            _unitOfWork.LessonRepository.Update(lesson);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
