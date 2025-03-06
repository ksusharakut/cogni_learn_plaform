using Application.Use_Cases.UserProgress.DTOs;

namespace Application.Use_Cases.UserProgress.GetCourseProgress
{
    public interface IGetCourseProgressUseCase
    {
        Task<CourseProgressDTO> ExecuteAsync(int courseId, CancellationToken cancellationToken);
    }
}
