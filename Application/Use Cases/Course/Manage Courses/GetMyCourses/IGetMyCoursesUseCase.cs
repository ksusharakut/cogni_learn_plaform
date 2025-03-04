using Application.Use_Cases.Course.DTOs;

namespace Application.Use_Cases.Course.Manage_Courses.GetMyCourses
{
    public interface IGetMyCoursesUseCase
    {
        Task<IEnumerable<CourseDTO>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
