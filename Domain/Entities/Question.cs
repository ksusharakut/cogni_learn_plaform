using Domain.Enums;

namespace Domain.Entities
{
    public class Question
    {
        public int QuestionId { get; set; }
        public int LessonId { get; set; }
        public string Text { get; set; }
        public QuestionType QuestionType { get; set; }
        public int OrderIndex { get; set; }
        public string CorrectAnswer { get; set; }

        public Lesson Lesson { get; set; }
        public ICollection<AnswerOption> AnswerOptions { get; set; }
    }
}
