using Domain.Enums;

namespace Application.Use_Cases.Question.DTOs
{
    public class QuestionDTO
    {
        public int QuestionId { get; set; }
        public int LessonId { get; set; }
        public string Text { get; set; }
        public QuestionType QuestionType { get; set; }
        public int OrderIndex { get; set; }
    }
}
