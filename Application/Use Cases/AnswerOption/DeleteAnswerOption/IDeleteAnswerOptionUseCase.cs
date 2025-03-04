namespace Application.Use_Cases.AnswerOption.DeleteAnswerOption
{
    public interface IDeleteAnswerOptionUseCase
    {
        Task ExecuteAsync(int answerOptionId, CancellationToken cancellationToken);
    }
}
