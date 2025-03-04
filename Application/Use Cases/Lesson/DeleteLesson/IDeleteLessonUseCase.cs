namespace Application.Use_Cases.Lesson.DeleteLesson
{
    public interface IDeleteLessonUseCase
    {
        Task ExecuteAsync(int lessonId, CancellationToken cancellationToken);
    }
}
