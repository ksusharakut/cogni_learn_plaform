using Application.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Lesson.DeleteLesson
{
    public class DeleteLessonUseCase : IDeleteLessonUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteLessonUseCase(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ExecuteAsync(int lessonId, CancellationToken cancellationToken)
        {
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
                throw new AuthorizationException("Вы можете удалять уроки только из своих курсов.");
            }

            _unitOfWork.LessonRepository.Delete(lesson);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
