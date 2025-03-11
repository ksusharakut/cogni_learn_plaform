using Application.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Chapter.DeleteChapter
{
    public class DeleteChapterUseCase : IDeleteChapterUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteChapterUseCase(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ExecuteAsync(int chapterId, CancellationToken cancellationToken)
        {
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
                throw new AuthorizationException("Вы можете удалять главы только из своих курсов.");
            }

            _unitOfWork.ChapterRepository.Delete(chapter);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
