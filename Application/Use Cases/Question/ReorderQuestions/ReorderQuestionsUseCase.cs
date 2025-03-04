using Application.Exceptions;
using Application.Use_Cases.Question.DTOs;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Question.ReorderQuestions
{
    public class ReorderQuestionsUseCase : IReorderQuestionsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ReorderQuestionsDTO> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReorderQuestionsUseCase(
            IUnitOfWork unitOfWork,
            IValidator<ReorderQuestionsDTO> validator,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task ExecuteAsync(int lessonId, ReorderQuestionsDTO request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

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

            var lesson = await _unitOfWork.LessonRepository.GetByIdWithChapterAsync(lessonId, cancellationToken);
            if (lesson == null)
            {
                throw new NotFoundException($"Урок с идентификатором {lessonId} не найден.");
            }

            if (lesson.Chapter.Course.UserId != userId)
            {
                throw new AuthorizationException("Вы можете переупорядочивать вопросы только в своих курсах.");
            }

            var questions = await _unitOfWork.QuestionRepository.GetByLessonIdAsync(lessonId, cancellationToken);
            var questionIds = questions.Select(q => q.QuestionId).ToList();

            var requestedIds = request.Questions.Select(q => q.QuestionId).ToList();
            if (!requestedIds.All(id => questionIds.Contains(id)) || requestedIds.Count != questionIds.Count)
            {
                throw new ValidationException("Список вопросов должен содержать все и только вопросы этого урока.");
            }

            foreach (var questionOrder in request.Questions)
            {
                var question = questions.First(q => q.QuestionId == questionOrder.QuestionId);
                question.OrderIndex = questionOrder.OrderIndex;
                _unitOfWork.QuestionRepository.Update(question);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

   
    }
}
