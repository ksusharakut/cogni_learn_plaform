using Domain.Enums;

namespace Domain.Entities
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public int ChapterId { get; set; }
        public string Name { get; set; }
        public string ContentPath { get; set; }
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public int OrderIndex { get; set; }
        public LessonType LessonType { get; set; }

        public Chapter Chapter { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
