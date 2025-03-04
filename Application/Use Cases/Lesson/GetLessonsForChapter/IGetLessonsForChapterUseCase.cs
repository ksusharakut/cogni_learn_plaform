using Application.Use_Cases.Lesson.DTOs;

namespace Application.Use_Cases.Lesson.GetLessonsForChapter
{
    public interface IGetLessonsForChapterUseCase
    {
        Task<IEnumerable<LessonDTO>> ExecuteAsync(int chapterId, CancellationToken cancellationToken);
    }
}
