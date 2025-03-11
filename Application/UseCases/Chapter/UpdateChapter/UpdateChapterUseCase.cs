using Application.Exceptions;
using Application.Use_Cases.Chapter.DTOs;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Chapter.UpdateChapter
{
    public class UpdateChapterUseCase : IUpdateChapterUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateChapterDTO> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateChapterUseCase(
            IUnitOfWork unitOfWork,
            IValidator<CreateChapterDTO> validator,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ExecuteAsync(int chapterId, CreateChapterDTO request, CancellationToken cancellationToken)
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

            var chapter = await _unitOfWork.ChapterRepository.GetByIdWithCourseAsync(chapterId, cancellationToken);
            if (chapter == null)
            {
                throw new NotFoundException($"Глава с идентификатором {chapterId} не найдена.");
            }

            if (chapter.Course.UserId != userId)
            {
                throw new AuthorizationException("Вы можете обновлять главы только в своих курсах.");
            }

            chapter.Title = request.Title;
            chapter.UpdatedAt = DateTimeOffset.UtcNow;

            _unitOfWork.ChapterRepository.Update(chapter);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
