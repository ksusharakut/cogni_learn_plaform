namespace Application.Use_Cases.Question.DeleteQuestion
{
    public interface IDeleteQuestionUseCase
    {
        Task ExecuteAsync(int questionId, CancellationToken cancellationToken);
    }
}
