using Application.Exceptions;
using Application.Use_Cases.UserProgress.DTOs;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.UserProgress.SubmitAnswer
{
    public class SubmitOpenEndedAnswerUseCase : ISubmitOpenEndedAnswerUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubmitOpenEndedAnswerUseCase(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SubmitAnswerResultDTO> ExecuteAsync(int questionId, SubmitOpenEndedAnswerDTO request, CancellationToken cancellationToken)
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

            if (question.QuestionType != QuestionType.OpenEnded)
            {
                throw new FluentValidation.ValidationException(new[]
                {
                    new ValidationFailure("QuestionType", "Этот use case предназначен только для открытых вопросов.")
                });
            }

            var courseId = question.Lesson.Chapter.CourseId;
            var chapterId = question.Lesson.ChapterId;
            var lessonId = question.LessonId;
            var isEnrolled = await _unitOfWork.UserCourseRepository.GetByUserAndCourseAsync(userId, courseId, cancellationToken) != null;
            if (!isEnrolled)
            {
                throw new AuthorizationException("Вы не записаны на этот курс.");
            }

            var progress = await _unitOfWork.UserProgressRepository.GetByUserAndQuestionAsync(userId, questionId, cancellationToken);
            if (progress != null && progress.IsCompleted)
            {
                throw new ConflictException("Вы уже ответили на этот вопрос.");
            }

            if (question.QuestionType != QuestionType.OpenEnded)
            {
                throw new FluentValidation.ValidationException(new[]
                {
                    new ValidationFailure("QuestionType", "Этот use case предназначен только для открытых вопросов.")
                });
            }

            bool isCorrect = string.Equals(request.TextAnswer.Trim(), question.CorrectAnswer?.Trim(), StringComparison.OrdinalIgnoreCase);
            if (progress == null)
            {
                progress = new Domain.Entities.UserProgress
                {
                    UserId = userId,
                    CourseId = courseId,
                    ChapterId = chapterId,
                    LessonId = lessonId,
                    QuestionId = questionId,
                    AnswerOptionId = null,
                    IsCorrect = isCorrect,
                    IsCompleted = true
                };
                await _unitOfWork.UserProgressRepository.AddAsync(progress, cancellationToken);
            }
            else
            {
                progress.IsCorrect = isCorrect;
                progress.IsCompleted = true;
                _unitOfWork.UserProgressRepository.Update(progress);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new SubmitAnswerResultDTO
            {
                IsCorrect = isCorrect,
                Message = isCorrect ? "Правильный ответ!" : "Неправильный ответ."
            };
        }
    }
}
