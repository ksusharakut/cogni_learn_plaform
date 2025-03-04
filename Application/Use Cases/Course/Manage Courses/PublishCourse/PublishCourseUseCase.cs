using Application.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Course.Manage_Courses.PublishCourse
{
    public class PublishCourseUseCase : IPublishCourseUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PublishCourseUseCase(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ExecuteAsync(int courseId, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new AuthenticationFailedException("Невозможно идентифицировать текущего пользователя.");
            }

            var course = await _unitOfWork.CourseRepository.GetByIdAsync(courseId, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException($"Курс с идентификатором {courseId} не найден.");
            }

            if (course.UserId != userId)
            {
                throw new AuthorizationException("Вы можете опубликовать только свой курс.");
            }

            if (course.IsPublished)
            {
                throw new ConflictException("Курс уже опубликован.");
            }

            course.IsPublished = true;
            course.UpdatedAt = DateTimeOffset.UtcNow;
            _unitOfWork.CourseRepository.Update(course);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
