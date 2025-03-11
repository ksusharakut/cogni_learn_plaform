using Application.Use_Cases.Chapter.DTOs;

namespace Application.Use_Cases.Chapter.CreateChapter
{
    public interface ICreateChapterUseCase
    {
        Task<int> ExecuteAsync(int id, CreateChapterDTO request, CancellationToken cancellationToken);
    }
}
