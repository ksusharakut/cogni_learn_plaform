namespace Application.Use_Cases.Course.Manage_Courses.DeleteCourse
{
    public interface IDeleteCourseUseCase
    {
        Task ExecuteAsync(int courseId, CancellationToken cancellationToken);
    }
}
