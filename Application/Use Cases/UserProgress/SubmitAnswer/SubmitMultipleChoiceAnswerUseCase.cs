using Application.Exceptions;
using Application.Use_Cases.UserProgress.DTOs;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using FluentValidation; 
using FluentValidation.Results;

namespace Application.Use_Cases.UserProgress.SubmitAnswer
{
    public class SubmitMultipleChoiceAnswerUseCase : ISubmitMultipleChoiceAnswerUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubmitMultipleChoiceAnswerUseCase(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SubmitAnswerResultDTO> ExecuteAsync(int questionId, SubmitMultipleChoiceAnswerDTO request, CancellationToken cancellationToken)
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

            if (question.QuestionType != QuestionType.MultipleChoice)
            {
                throw new FluentValidation.ValidationException(new[]
                {
                    new ValidationFailure("QuestionType", "Этот use case предназначен только для вопросов с множественным выбором.")
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

            if (!request.AnswerOptionId.HasValue)
            {
                throw new FluentValidation.ValidationException(new[]
                {
                    new ValidationFailure("AnswerOptionId", "Для вопросов с множественным выбором необходимо указать AnswerOptionId.")
                });
            }

            var answerOption = await _unitOfWork.AnswerOptionRepository.GetByIdAsync(request.AnswerOptionId.Value, cancellationToken);
            if (answerOption == null || answerOption.QuestionId != questionId)
            {
                throw new NotFoundException($"Вариант ответа с идентификатором {request.AnswerOptionId} не найден или не относится к этому вопросу.");
            }

            bool isCorrect = answerOption.IsCorrect;
            if (progress == null)
            {
                progress = new Domain.Entities.UserProgress
                {
                    UserId = userId,
                    CourseId = courseId,
                    ChapterId = chapterId,
                    LessonId = lessonId,
                    QuestionId = questionId,
                    AnswerOptionId = request.AnswerOptionId,
                    IsCorrect = isCorrect,
                    IsCompleted = true
                };
                await _unitOfWork.UserProgressRepository.AddAsync(progress, cancellationToken);
            }
            else
            {
                progress.AnswerOptionId = request.AnswerOptionId;
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
