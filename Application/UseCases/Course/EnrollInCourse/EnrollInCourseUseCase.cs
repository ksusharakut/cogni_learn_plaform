using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Course.EnrollInCourse
{
    public class EnrollInCourseUseCase : IEnrollInCourseUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EnrollInCourseUseCase(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
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

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException($"Пользователь с идентификатором {userId} не найден.");
            }

            var course = await _unitOfWork.CourseRepository.GetByIdAsync(courseId, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException($"Курс с идентификатором {courseId} не найден.");
            }

            if (!course.IsPublished)
            {
                throw new AuthorizationException("Нельзя записаться на неопубликованный курс.");
            }

            // Проверяем, не записан ли пользователь уже
            var existingEnrollment = await _unitOfWork.UserCourseRepository.GetByUserAndCourseAsync(userId, courseId, cancellationToken);
            if (existingEnrollment != null)
            {
                throw new ConflictException("Вы уже записаны на этот курс.");
            }

            var userCourse = new UserCourse
            {
                UserId = userId,
                CourseId = courseId
            };

            await _unitOfWork.UserCourseRepository.AddAsync(userCourse, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
