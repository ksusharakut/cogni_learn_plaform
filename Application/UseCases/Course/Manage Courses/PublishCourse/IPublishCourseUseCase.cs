namespace Application.Use_Cases.Course.Manage_Courses.PublishCourse
{
    public interface IPublishCourseUseCase
    {
        Task ExecuteAsync(int courseId, CancellationToken cancellationToken);
    }
}
