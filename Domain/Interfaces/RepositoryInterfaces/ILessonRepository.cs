using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface ILessonRepository
    {
        Task AddAsync(Lesson lesson, CancellationToken cancellationToken);
        Task<Lesson> GetByIdAsync(int lessonId, CancellationToken cancellationToken);
        Task<Lesson> GetByIdWithChapterAsync(int lessonId, CancellationToken cancellationToken);
        void Update(Lesson lesson);
        void Delete(Lesson lesson);
        Task<int> GetMaxOrderIndexAsync(int chapterId, CancellationToken cancellationToken);
        Task<List<Lesson>> GetByChapterIdAsync(int chapterId, CancellationToken cancellationToken);
    }
}
