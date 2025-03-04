using Domain.Enums;

namespace Application.Use_Cases.Lesson.DTOs
{
    public class LessonDTO
    {
        public int LessonId { get; set; }
        public int ChapterId { get; set; }
        public string Name { get; set; }
        public string ContentPath { get; set; }
        public int OrderIndex { get; set; }
        public LessonType LessonType { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
