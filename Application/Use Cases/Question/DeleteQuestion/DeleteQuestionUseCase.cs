using Application.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Question.DeleteQuestion
{
    public class DeleteQuestionUseCase : IDeleteQuestionUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteQuestionUseCase(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ExecuteAsync(int questionId, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new AuthenticationFailedException("Невозможно идентифицировать текущего пользователя.");
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null || !user.Roles.Any(r => r.RoleName == "Author"))
            {
                throw new AuthorizationException("Пользователь не имеет роль 'Author'.");
            }

            var question = await _unitOfWork.QuestionRepository.GetByIdWithLessonAsync(questionId, cancellationToken);
            if (question == null)
            {
                throw new NotFoundException($"Вопрос с идентификатором {questionId} не найден.");
            }

            if (question.Lesson.Chapter.Course.UserId != userId)
            {
                throw new AuthorizationException("Вы можете удалять вопросы только из своих курсов.");
            }

            _unitOfWork.QuestionRepository.Delete(question);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
