namespace Domain.Entities
{
    public class UserCourse
    {
        public int UserCourseId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public bool CompletionStatus { get; set; }

        public Course Course { get; set; }
        public User User { get; set; }
    }
}
