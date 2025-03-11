using Application.Use_Cases.Chapter.DTOs;

namespace Application.Use_Cases.Course.DTOs
{
    public class CourseDetailsDTO
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublished { get; set; }
        public string AuthorEmail { get; set; }
        public double AverageRating { get; set; }
        public int RatingCount { get; set; }
        public List<string> Categories { get; set; }
        public List<ChapterDetailsDTO> Chapters { get; set; }
    }
}
