using Application.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.UserProgress.CompleteLesson
{
    public class CompleteLessonUseCase : ICompleteLessonUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompleteLessonUseCase(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
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
            if (user == null)
            {
                throw new NotFoundException($"Пользователь с идентификатором {userId} не найден.");
            }

            var lesson = await _unitOfWork.LessonRepository.GetByIdWithChapterAsync(lessonId, cancellationToken);
            if (lesson == null)
            {
                throw new NotFoundException($"Урок с идентификатором {lessonId} не найден.");
            }

            var courseId = lesson.Chapter.CourseId;
            var chapterId = lesson.ChapterId;
            var isEnrolled = await _unitOfWork.UserCourseRepository.GetByUserAndCourseAsync(userId, courseId, cancellationToken) != null;
            if (!isEnrolled)
            {
                throw new AuthorizationException("Вы не записаны на этот курс.");
            }

            var lessonProgress = await _unitOfWork.UserProgressRepository.GetByUserAndLessonAsync(userId, lessonId, cancellationToken);
            if (lessonProgress == null)
            {
                lessonProgress = new Domain.Entities.UserProgress
                {
                    UserId = userId,
                    CourseId = courseId,
                    ChapterId = chapterId,
                    LessonId = lessonId,
                    IsCompleted = true
                };
                await _unitOfWork.UserProgressRepository.AddAsync(lessonProgress, cancellationToken);
            }
            else if (!lessonProgress.IsCompleted)
            {
                lessonProgress.IsCompleted = true;
                _unitOfWork.UserProgressRepository.Update(lessonProgress);
            }

            var chapterLessons = await _unitOfWork.LessonRepository.GetByChapterIdAsync(chapterId, cancellationToken);
            var chapterProgressList = await _unitOfWork.UserProgressRepository.GetByUserAndCourseAsync(userId, courseId, cancellationToken);
            var allLessonsCompleted = chapterLessons.All(l =>
                chapterProgressList.Any(p => p.LessonId == l.LessonId && p.IsCompleted));

            if (allLessonsCompleted)
            {
                var chapterProgress = await _unitOfWork.UserProgressRepository.GetByUserAndChapterAsync(userId, chapterId, cancellationToken);
                if (chapterProgress == null)
                {
                    chapterProgress = new Domain.Entities.UserProgress
                    {
                        UserId = userId,
                        CourseId = courseId,
                        ChapterId = chapterId,
                        IsCompleted = true,
                    };
                    await _unitOfWork.UserProgressRepository.AddAsync(chapterProgress, cancellationToken);
                }
                else if (!chapterProgress.IsCompleted)
                {
                    chapterProgress.IsCompleted = true;
                    _unitOfWork.UserProgressRepository.Update(chapterProgress);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
