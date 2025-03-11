using Application.Exceptions;
using Application.Use_Cases.UserProgress.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.UserProgress.GetCourseProgress
{
    public class GetCourseProgressUseCase : IGetCourseProgressUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetCourseProgressUseCase(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CourseProgressDTO> ExecuteAsync(int courseId, CancellationToken cancellationToken)
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

            var course = await _unitOfWork.CourseRepository.GetByIdWithDetailsAsync(courseId, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException($"Курс с идентификатором {courseId} не найден.");
            }

            var isEnrolled = await _unitOfWork.UserCourseRepository.GetByUserAndCourseAsync(userId, courseId, cancellationToken) != null;
            if (!isEnrolled)
            {
                throw new AuthorizationException("Вы не записаны на этот курс.");
            }

            var progress = await _unitOfWork.UserProgressRepository.GetByUserAndCourseAsync(userId, courseId, cancellationToken);
            var progressList = progress.ToList();

            var totalChapters = course.Chapters.Count;
            var completedChapters = progressList.Count(p => p.ChapterId.HasValue && !p.LessonId.HasValue && p.IsCompleted);
            var totalLessons = course.Chapters.SelectMany(c => c.Lessons).Count();
            var completedLessons = progressList.Count(p => p.LessonId.HasValue && p.IsCompleted);
            var totalQuestions = course.Chapters.SelectMany(c => c.Lessons).SelectMany(l => l.Questions).Count();
            var correctAnswers = progressList.Count(p => p.QuestionId.HasValue && p.IsCorrect);
            var completionPercentage = totalLessons > 0 ? (double)completedLessons / totalLessons * 100 : 0;
            var lastCompletedAt = progressList.Where(p => p.IsCompleted).Max(p => (DateTimeOffset?)p.CompletedAt);

            return new CourseProgressDTO
            {
                CourseId = courseId,
                TotalChapters = totalChapters,
                CompletedChapters = completedChapters,
                TotalLessons = totalLessons,
                CompletedLessons = completedLessons,
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctAnswers,
                CompletionPercentage = Math.Round(completionPercentage, 2)
            };
        }
    }
}
