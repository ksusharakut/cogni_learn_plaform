using Application.Use_Cases.Question.DTOs;

namespace Application.Use_Cases.Question.GetQuestion
{
    public interface IGetQuestionUseCase
    {
        Task<QuestionDTO> ExecuteAsync(int questionId, CancellationToken cancellationToken);
    }
}
