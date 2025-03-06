using Application.Use_Cases.Question.DTOs;

namespace Application.Use_Cases.Question.CreateQuestion
{
    public interface ICreateMultipleChoiceQuestionUseCase
    {
        Task<int> ExecuteAsync(int lessonId, CreateMultipleChoiceQuestionDTO request, CancellationToken cancellationToken);
    }
}
