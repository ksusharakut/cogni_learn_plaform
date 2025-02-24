using System.Transactions;

namespace Domain.Entities
{
    public class Course
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublished { get; set; }

        public virtual User User { get; set; }
        public ICollection<UserCourse> UserCourses { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public ICollection<Chapter> Chapters { get; set; }
    }
}
