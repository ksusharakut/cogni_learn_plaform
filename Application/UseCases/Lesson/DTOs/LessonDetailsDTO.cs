using Domain.Enums;

namespace Application.Use_Cases.Lesson.DTOs
{
    public class LessonDetailsDTO
    {
        public int LessonId { get; set; }
        public string Name { get; set; }
        public string ContentPath { get; set; }
        public int OrderIndex { get; set; }
        public LessonType LessonType { get; set; }
    }
}
