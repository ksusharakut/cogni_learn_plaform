using Application.Use_Cases.Lesson.DTOs;

namespace Application.Use_Cases.Lesson.GetLesson
{
    public interface IGetLessonUseCase
    {
        Task<LessonDTO> ExecuteAsync(int lessonId, CancellationToken cancellationToken);
    }
}
