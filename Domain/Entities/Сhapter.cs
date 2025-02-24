namespace Domain.Entities
{
    public class Chapter
    {
        public int ChapterId { get; set; }
        public int CourseId { get; set; }
        public int OrderIndex { get; set; }
        public string Title { get; set; }
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public Course Course { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
    }
}
