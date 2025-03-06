namespace Application.Use_Cases.UserProgress.CompleteLesson
{
    public interface ICompleteLessonUseCase
    {
        Task ExecuteAsync(int lessonId, CancellationToken cancellationToken);
    }
}
