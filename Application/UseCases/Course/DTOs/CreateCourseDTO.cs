namespace Application.Use_Cases.Course.DTOs
{
    public class CreateCourseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
