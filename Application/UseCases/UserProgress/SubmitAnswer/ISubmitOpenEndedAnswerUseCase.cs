using Application.Use_Cases.UserProgress.DTOs;

namespace Application.Use_Cases.UserProgress.SubmitAnswer
{
    public interface ISubmitOpenEndedAnswerUseCase
    {
        Task<SubmitAnswerResultDTO> ExecuteAsync(int questionId, SubmitOpenEndedAnswerDTO request, CancellationToken cancellationToken);
    }
}
