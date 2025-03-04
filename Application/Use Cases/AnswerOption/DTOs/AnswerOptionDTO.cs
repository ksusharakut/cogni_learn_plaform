namespace Application.Use_Cases.AnswerOption.DTOs
{
    public class AnswerOptionDTO
    {
        public int AnswerOptionId { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
