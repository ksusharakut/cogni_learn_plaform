using Application.Exceptions;
using Application.Use_Cases.Lesson.DTOs;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Lesson.GetLesson
{
    public class GetLessonUseCase : IGetLessonUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetLessonUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LessonDTO> ExecuteAsync(int lessonId, CancellationToken cancellationToken)
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

            var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(lessonId, cancellationToken);
            if (lesson == null)
            {
                throw new NotFoundException($"Урок с идентификатором {lessonId} не найден.");
            }

            var course = await _unitOfWork.CourseRepository.GetByIdAsync(lesson.Chapter.CourseId, cancellationToken);
            if (course == null || (!course.IsPublished && course.UserId != userId))
            {
                throw new AuthorizationException("У вас нет доступа к этому уроку.");
            }

            return _mapper.Map<LessonDTO>(lesson);
        }
    }
}
