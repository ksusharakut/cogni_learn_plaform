using Application.Exceptions;
using Application.Use_Cases.Lesson.DTOs;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Lesson.GetLessonsForChapter
{
    public class GetLessonsForChapterUseCase : IGetLessonsForChapterUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetLessonsForChapterUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<LessonDTO>> ExecuteAsync(int chapterId, CancellationToken cancellationToken)
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

            var chapter = await _unitOfWork.ChapterRepository.GetByIdWithCourseAsync(chapterId, cancellationToken);
            if (chapter == null)
            {
                throw new NotFoundException($"Глава с идентификатором {chapterId} не найдена.");
            }

            var course = chapter.Course;
            if (!course.IsPublished && course.UserId != userId)
            {
                throw new AuthorizationException("У вас нет доступа к урокам этого курса.");
            }

            var lessons = await _unitOfWork.LessonRepository.GetByChapterIdAsync(chapterId, cancellationToken);
            return _mapper.Map<IEnumerable<LessonDTO>>(lessons);
        }
    }
}
