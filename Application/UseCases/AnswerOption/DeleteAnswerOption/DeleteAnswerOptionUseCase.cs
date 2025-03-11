using Application.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.AnswerOption.DeleteAnswerOption
{
    public class DeleteAnswerOptionUseCase : IDeleteAnswerOptionUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteAnswerOptionUseCase(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ExecuteAsync(int answerOptionId, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new AuthenticationFailedException("Невозможно идентифицировать текущего пользователя.");
            }

            var answerOption = await _unitOfWork.AnswerOptionRepository.GetByIdWithQuestionAsync(answerOptionId, cancellationToken);
            if (answerOption == null)
            {
                throw new NotFoundException($"Вариант ответа с идентификатором {answerOptionId} не найден.");
            }

            if (answerOption.Question.Lesson.Chapter.Course.UserId != userId)
            {
                throw new AuthorizationException("Вы можете удалять варианты ответа только из своих курсов.");
            }

            _unitOfWork.AnswerOptionRepository.Delete(answerOption);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
