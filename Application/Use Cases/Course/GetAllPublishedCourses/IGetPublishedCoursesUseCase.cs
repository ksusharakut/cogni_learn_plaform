using Application.Use_Cases.Course.DTOs;

namespace Application.Use_Cases.Course.GetAllCourses
{
    public interface IGetPublishedCoursesUseCase
    {
        Task<IEnumerable<CourseDTO>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
