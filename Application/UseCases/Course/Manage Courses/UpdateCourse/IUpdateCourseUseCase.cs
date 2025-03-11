using Application.Use_Cases.Course.DTOs;

namespace Application.Use_Cases.Course.Manage_Courses.UpdateCourse
{
    public interface IUpdateCourseUseCase
    {
        Task ExecuteAsync(int id, CreateCourseDTO request, CancellationToken cancellationToken);
    }
}
