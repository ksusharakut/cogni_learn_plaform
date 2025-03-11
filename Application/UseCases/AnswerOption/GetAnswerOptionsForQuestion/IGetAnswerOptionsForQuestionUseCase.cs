using Application.Use_Cases.AnswerOption.DTOs;

namespace Application.Use_Cases.AnswerOption.GetAnswerOptionsForQuestion
{
    public interface IGetAnswerOptionsForQuestionUseCase
    {
        Task<IEnumerable<AnswerOptionDTO>> ExecuteAsync(int questionId, CancellationToken cancellationToken);
    }
}
