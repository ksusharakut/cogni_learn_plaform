using Application.Use_Cases.Question.DTOs;

namespace Application.Use_Cases.Question.CreateQuestion
{
    public interface ICreateOpenEndedQuestionUseCase
    {
        Task<int> ExecuteAsync(int lessonId, CreateOpenEndedQuestionDTO request, CancellationToken cancellationToken);  
    }
}
