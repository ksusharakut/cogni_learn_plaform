using Application.Use_Cases.Question.DTOs;

namespace Application.Use_Cases.Question.UpdateQuestion
{
    public interface IUpdateOpenEndedQuestionUseCase
    {
        Task ExecuteAsync(int questionId, CreateOpenEndedQuestionDTO request, CancellationToken cancellationToken);
    }
}
