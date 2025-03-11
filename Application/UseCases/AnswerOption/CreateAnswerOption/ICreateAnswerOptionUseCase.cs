using Application.Use_Cases.AnswerOption.DTOs;

namespace Application.Use_Cases.AnswerOption.CreateAnswerOption
{
    public interface ICreateAnswerOptionUseCase
    {
        Task<int> ExecuteAsync(int questionId, CreateAnswerOptionDTO request, CancellationToken cancellationToken);
    }
}
