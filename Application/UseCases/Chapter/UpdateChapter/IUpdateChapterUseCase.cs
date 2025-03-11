using Application.Use_Cases.Chapter.DTOs;

namespace Application.Use_Cases.Chapter.UpdateChapter
{
    public interface IUpdateChapterUseCase
    {
        Task ExecuteAsync(int chapterId, CreateChapterDTO request, CancellationToken cancellationToken);
    }
}
