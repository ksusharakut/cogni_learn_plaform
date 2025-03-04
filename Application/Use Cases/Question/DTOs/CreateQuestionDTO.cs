using Domain.Enums;

namespace Application.Use_Cases.Question.DTOs
{
    public class CreateQuestionDTO
    {
        public string Text { get; set; }
        public QuestionType QuestionType { get; set; }
    }
}
