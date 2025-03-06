namespace Application.Use_Cases.UserProgress.DTOs
{
    public class CourseProgressDTO
    {
        public int CourseId { get; set; }
        public int TotalChapters { get; set; }
        public int CompletedChapters { get; set; }
        public int TotalLessons { get; set; }
        public int CompletedLessons { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public double CompletionPercentage { get; set; }
    }
}
