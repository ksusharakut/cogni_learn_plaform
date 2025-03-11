using Application.Use_Cases.Lesson.DTOs;

namespace Application.Use_Cases.Lesson.ReorderLessons
{
    public interface IReorderLessonsUseCase
    {
        Task ExecuteAsync(int chapterId, ReorderLessonsDTO request, CancellationToken cancellationToken);
    }
}
