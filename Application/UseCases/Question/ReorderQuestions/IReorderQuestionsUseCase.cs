using Application.Use_Cases.Question.DTOs;

namespace Application.Use_Cases.Question.ReorderQuestions
{
    public interface IReorderQuestionsUseCase
    {
        Task ExecuteAsync(int lessonId, ReorderQuestionsDTO request, CancellationToken cancellationToken);
    }
}
