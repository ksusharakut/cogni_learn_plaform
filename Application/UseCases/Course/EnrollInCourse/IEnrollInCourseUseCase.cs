namespace Application.Use_Cases.Course.EnrollInCourse
{
    public interface IEnrollInCourseUseCase
    {
        Task ExecuteAsync(int courseId, CancellationToken cancellationToken);
    }
}
