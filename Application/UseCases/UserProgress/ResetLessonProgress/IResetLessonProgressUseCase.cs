namespace Application.Use_Cases.UserProgress
{
    public interface IResetLessonProgressUseCase
    {
        Task ExecuteAsync(int lessonId, CancellationToken cancellationToken);
    }
}
