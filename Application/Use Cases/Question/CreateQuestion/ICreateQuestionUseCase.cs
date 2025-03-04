using Application.Use_Cases.Question.DTOs;

namespace Application.Use_Cases.Question.CreateQuestion
{
    public interface ICreateQuestionUseCase
    {
        Task<int> ExecuteAsync(int lessonId, CreateQuestionDTO request, CancellationToken cancellationToken);
    }
}
