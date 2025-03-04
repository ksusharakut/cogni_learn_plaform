using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface IAnswerOptionRepository
    {
        Task AddAsync(AnswerOption answerOption, CancellationToken cancellationToken);
        Task<AnswerOption> GetByIdWithQuestionAsync(int answerOptionId, CancellationToken cancellationToken);
        void Update(AnswerOption answerOption);
        void Delete(AnswerOption answerOption);
        Task<List<AnswerOption>> GetByQuestionIdAsync(int questionId, CancellationToken cancellationToken);
    }
}
