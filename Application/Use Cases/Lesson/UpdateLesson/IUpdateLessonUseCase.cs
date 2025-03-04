namespace Application.Use_Cases.Lesson.UpdateLesson
{
    public interface IUpdateLessonUseCase
    {
        Task ExecuteAsync(int lessonId, CreateLessonDTO request, CancellationToken cancellationToken);
    }
}
