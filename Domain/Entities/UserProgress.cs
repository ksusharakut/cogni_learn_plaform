namespace Domain.Entities
{
    public class UserProgress
    {
        public int UserProgressId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public int? ChapterId { get; set; } 
        public int? LessonId { get; set; } 
        public int? QuestionId { get; set; } 
        public int? AnswerOptionId { get; set; } 
        public bool IsCompleted { get; set; } 
        public bool IsCorrect { get; set; } 
        public DateTimeOffset CompletedAt { get; set; } = DateTimeOffset.UtcNow;

        public User User { get; set; }
        public Course Course { get; set; }
        public Chapter Chapter { get; set; }
        public Lesson Lesson { get; set; }
        public Question Question { get; set; }
        public AnswerOption AnswerOption { get; set; }
    }
}
