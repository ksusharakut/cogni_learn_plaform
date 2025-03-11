using Application.Use_Cases.AnswerOption.DTOs;

namespace Application.Use_Cases.Question.DTOs
{
    public class CreateMultipleChoiceQuestionDTO
    {
        public string Text { get; set; }
        public List<CreateAnswerOptionDTO> AnswerOptions { get; set; }
    }
}
