using Domain.Enums;

namespace Application.Use_Cases.Lesson
{
    public class CreateLessonDTO
    {
        public string Name { get; set; }
        public string ContentPath { get; set; }
        public LessonType LessonType { get; set; }
    }
}
