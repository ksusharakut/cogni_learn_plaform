using Application.Exceptions;
using Application.Use_Cases.Question.DTOs;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Question.GetQuestionsForLesson
{
    public class GetQuestionsForLessonUseCase : IGetQuestionsForLessonUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetQuestionsForLessonUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<QuestionDTO>> ExecuteAsync(int lessonId, CancellationToken cancellationToken)
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

            var course = lesson.Chapter.Course;
            var isEnrolled = await _unitOfWork.UserCourseRepository.GetByUserAndCourseAsync(userId, course.CourseId, cancellationToken) != null;
            if (!course.IsPublished && course.UserId != userId && !isEnrolled)
            {
                throw new AuthorizationException("У вас нет доступа к вопросам этого урока.");
            }

            var questions = await _unitOfWork.QuestionRepository.GetByLessonIdAsync(lessonId, cancellationToken);
            return _mapper.Map<IEnumerable<QuestionDTO>>(questions);
        }
    }
}
