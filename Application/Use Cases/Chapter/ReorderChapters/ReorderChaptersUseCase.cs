using Application.Exceptions;
using Application.Use_Cases.Chapter.DTOs;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Chapter.ReorderChapters
{
    public class ReorderChaptersUseCase : IReorderChaptersUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ReorderChaptersDTO> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReorderChaptersUseCase(
            IUnitOfWork unitOfWork,
            IValidator<ReorderChaptersDTO> validator,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ExecuteAsync(int courseId, ReorderChaptersDTO request, CancellationToken cancellationToken)
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

            var course = await _unitOfWork.CourseRepository.GetByIdAsync(courseId, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException($"Курс с идентификатором {courseId} не найден.");
            }

            if (course.UserId != userId)
            {
                throw new AuthorizationException("Вы можете переупорядочивать главы только в своих курсах.");
            }

            var chapters = await _unitOfWork.ChapterRepository.GetByCourseIdAsync(courseId, cancellationToken);
            var chapterIds = chapters.Select(c => c.ChapterId).ToList();

            var requestedIds = request.Chapters.Select(c => c.ChapterId).ToList();
            if (!requestedIds.All(id => chapterIds.Contains(id)) || requestedIds.Count != chapterIds.Count)
            {
                throw new ValidationException("Список глав должен содержать все и только главы этого курса.");
            }

            foreach (var chapterOrder in request.Chapters)
            {
                var chapter = chapters.First(c => c.ChapterId == chapterOrder.ChapterId);
                chapter.OrderIndex = chapterOrder.OrderIndex;
                chapter.UpdatedAt = DateTimeOffset.UtcNow;
                _unitOfWork.ChapterRepository.Update(chapter);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
