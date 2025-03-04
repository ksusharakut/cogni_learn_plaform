namespace Application.Use_Cases.Course.Manage_Courses.UnpublishCourse
{
    public interface IUnpublishCourseUseCase
    {
        Task ExecuteAsync(int courseId, CancellationToken cancellationToken);
    }
}
