using Application.Use_Cases.Lesson.DTOs;

namespace Application.Use_Cases.Chapter.DTOs
{
    public class ChapterDetailsDTO
    {
        public int ChapterId { get; set; }
        public string Title { get; set; }
        public int OrderIndex { get; set; }
        public List<LessonDetailsDTO> Lessons { get; set; }
    }
}
