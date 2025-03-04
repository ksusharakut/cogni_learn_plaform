using Application.Exceptions;
using Application.Use_Cases.AnswerOption.DTOs;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.AnswerOption.GetAnswerOption
{
    public class GetAnswerOptionUseCase : IGetAnswerOptionUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAnswerOptionUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AnswerOptionDTO> ExecuteAsync(int answerOptionId, CancellationToken cancellationToken)
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

            var answerOption = await _unitOfWork.AnswerOptionRepository.GetByIdWithQuestionAsync(answerOptionId, cancellationToken);
            if (answerOption == null)
            {
                throw new NotFoundException($"Вариант ответа с идентификатором {answerOptionId} не найден.");
            }

            var course = answerOption.Question.Lesson.Chapter.Course;
            var isEnrolled = await _unitOfWork.UserCourseRepository.GetByUserAndCourseAsync(userId, course.CourseId, cancellationToken) != null;
            if (!course.IsPublished && course.UserId != userId && !isEnrolled)
            {
                throw new AuthorizationException("У вас нет доступа к этому варианту ответа.");
            }

            return _mapper.Map<AnswerOptionDTO>(answerOption);
        }
    }
}
