using Application.Use_Cases.Course.DTOs;

namespace Application.Use_Cases.Course.GetMyEnrolledCourses
{
    public interface IGetMyEnrolledCoursesUseCase
    {
        Task<IEnumerable<CourseDTO>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
