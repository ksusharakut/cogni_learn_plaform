using Application.Use_Cases.AnswerOption.DTOs;

namespace Application.Use_Cases.AnswerOption.GetAnswerOption
{
    public interface IGetAnswerOptionUseCase
    {
        Task<AnswerOptionDTO> ExecuteAsync(int answerOptionId, CancellationToken cancellationToken);
    }
}
