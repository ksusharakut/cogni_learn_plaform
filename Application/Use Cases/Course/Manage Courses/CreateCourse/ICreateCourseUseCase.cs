using Application.Use_Cases.Course.DTOs;

namespace Application.Use_Cases.Course.CreateCourse
{
    public interface ICreateCourseUseCase
    {
        Task<int> ExecuteAsync(CreateCourseDTO request, CancellationToken cancellationToken);
    }
}
