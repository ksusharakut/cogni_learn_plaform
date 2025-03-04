using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface IQuestionRepository
    {
        Task AddAsync(Question question, CancellationToken cancellationToken);
        Task<Question> GetByIdAsync(int questionId, CancellationToken cancellationToken);
        Task<Question> GetByIdWithLessonAsync(int questionId, CancellationToken cancellationToken);
        void Update(Question question);
        void Delete(Question question);
        Task<int> GetMaxOrderIndexAsync(int lessonId, CancellationToken cancellationToken);
        Task<List<Question>> GetByLessonIdAsync(int lessonId, CancellationToken cancellationToken);
    }
}
