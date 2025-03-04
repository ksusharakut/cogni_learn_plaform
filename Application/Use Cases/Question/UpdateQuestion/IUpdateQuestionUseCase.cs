using Application.Use_Cases.Question.DTOs;

namespace Application.Use_Cases.Question.UpdateQuestion
{
    public interface IUpdateQuestionUseCase
    {
        Task ExecuteAsync(int questionId, CreateQuestionDTO request, CancellationToken cancellationToken);
    }
}
