using Application.Exceptions;
using Application.Use_Cases.Lesson.DTOs;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Lesson.ReorderLessons
{
    public class ReorderLessonsUseCase : IReorderLessonsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ReorderLessonsDTO> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReorderLessonsUseCase(
            IUnitOfWork unitOfWork,
            IValidator<ReorderLessonsDTO> validator,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ExecuteAsync(int chapterId, ReorderLessonsDTO request, CancellationToken cancellationToken)
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

            var chapter = await _unitOfWork.ChapterRepository.GetByIdWithCourseAsync(chapterId, cancellationToken);
            if (chapter == null)
            {
                throw new NotFoundException($"Глава с идентификатором {chapterId} не найдена.");
            }

            if (chapter.Course.UserId != userId)
            {
                throw new AuthorizationException("Вы можете переупорядочивать уроки только в своих курсах.");
            }

            var lessons = await _unitOfWork.LessonRepository.GetByChapterIdAsync(chapterId, cancellationToken);
            var lessonIds = lessons.Select(l => l.LessonId).ToList();

            var requestedIds = request.Lessons.Select(l => l.LessonId).ToList();
            if (!requestedIds.All(id => lessonIds.Contains(id)) || requestedIds.Count != lessonIds.Count)
            {
                throw new ValidationException("Список уроков должен содержать все и только уроки этой главы.");
            }

            foreach (var lessonOrder in request.Lessons)
            {
                var lesson = lessons.First(l => l.LessonId == lessonOrder.LessonId);
                lesson.OrderIndex = lessonOrder.OrderIndex;
                lesson.UpdatedAt = DateTimeOffset.UtcNow;
                _unitOfWork.LessonRepository.Update(lesson);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
