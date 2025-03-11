using Application.Use_Cases.Chapter.DTOs;

namespace Application.Use_Cases.Chapter.ReorderChapters
{
    public interface IReorderChaptersUseCase
    {
        Task ExecuteAsync(int courseId, ReorderChaptersDTO request, CancellationToken cancellationToken);
    }
}
