namespace Application.Use_Cases.Chapter.DeleteChapter
{
    public interface IDeleteChapterUseCase
    {
        Task ExecuteAsync(int chapterId, CancellationToken cancellationToken);
    }
}
