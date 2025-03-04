using Application.Use_Cases.Course.DTOs;

namespace Application.Use_Cases.Course.GetCourseWithDetails
{
    public interface IGetCourseWithDetailsUseCase
    {
        Task<CourseDetailsDTO> ExecuteAsync(int courseId, CancellationToken cancellationToken);
    }
}
