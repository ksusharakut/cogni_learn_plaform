using Application.Use_Cases.AnswerOption.DTOs;

namespace Application.Use_Cases.AnswerOption.UpdateAnswerOption
{
    public interface IUpdateAnswerOptionUseCase
    {
        Task ExecuteAsync(int answerOptionId, CreateAnswerOptionDTO request, CancellationToken cancellationToken);
    }
}
