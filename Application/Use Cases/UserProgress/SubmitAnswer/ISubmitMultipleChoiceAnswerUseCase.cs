using Application.Use_Cases.UserProgress.DTOs;

namespace Application.Use_Cases.UserProgress.SubmitAnswer
{
    public interface ISubmitMultipleChoiceAnswerUseCase
    {
        Task<SubmitAnswerResultDTO> ExecuteAsync(int questionId, SubmitMultipleChoiceAnswerDTO request, CancellationToken cancellationToken);
    }
}
