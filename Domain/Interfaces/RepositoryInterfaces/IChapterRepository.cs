using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface IChapterRepository
    {
        Task AddAsync(Chapter chapter, CancellationToken cancellationToken);
        void Delete(Chapter chapter);
        Task<int> GetMaxOrderIndexAsync(int courseId, CancellationToken cancellationToken);
        void Update(Chapter chapter);
        Task<Chapter> GetByIdWithCourseAsync(int chapterId, CancellationToken cancellationToken);
        Task<List<Chapter>> GetByCourseIdAsync(int courseId, CancellationToken cancellationToken);
    }
}
