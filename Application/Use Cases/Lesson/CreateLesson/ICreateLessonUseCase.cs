namespace Application.Use_Cases.Lesson.CreateLesson
{
    public interface ICreateLessonUseCase
    {
        Task<int> ExecuteAsync(int chapterId, CreateLessonDTO request, CancellationToken cancellationToken);
    }
}
