using Application.Exceptions;
using Application.Use_Cases.AnswerOption.DTOs;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.AnswerOption.GetAnswerOptionsForQuestion
{
    public class GetAnswerOptionsForQuestionUseCase : IGetAnswerOptionsForQuestionUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAnswerOptionsForQuestionUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<AnswerOptionDTO>> ExecuteAsync(int questionId, CancellationToken cancellationToken)
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

            var question = await _unitOfWork.QuestionRepository.GetByIdWithLessonAsync(questionId, cancellationToken);
            if (question == null)
            {
                throw new NotFoundException($"Вопрос с идентификатором {questionId} не найден.");
            }

            var course = question.Lesson.Chapter.Course;
            var isEnrolled = await _unitOfWork.UserCourseRepository.GetByUserAndCourseAsync(userId, course.CourseId, cancellationToken) != null;
            if (!course.IsPublished && course.UserId != userId && !isEnrolled)
            {
                throw new AuthorizationException("У вас нет доступа к вариантам ответа этого вопроса.");
            }

            var answerOptions = await _unitOfWork.AnswerOptionRepository.GetByQuestionIdAsync(questionId, cancellationToken);
            return _mapper.Map<IEnumerable<AnswerOptionDTO>>(answerOptions);
        }
    }
}
