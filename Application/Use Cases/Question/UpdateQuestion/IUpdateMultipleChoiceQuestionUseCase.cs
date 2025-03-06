using Application.Use_Cases.Question.DTOs;

namespace Application.Use_Cases.Question.UpdateQuestion
{
    public interface IUpdateMultipleChoiceQuestionUseCase
    {
        Task ExecuteAsync(int questionId, CreateMultipleChoiceQuestionDTO request, CancellationToken cancellationToken);
    }
}
