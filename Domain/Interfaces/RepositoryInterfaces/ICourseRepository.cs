using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface ICourseRepository
    {
        Task AddAsync(Course course, CancellationToken cancellationToken);
        Task<Course?> GetByIdAsync(int courseId, CancellationToken cancellationToken);
        Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Course>> GetAllPublishedAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Course>> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task<Course> GetByIdWithDetailsAsync(int courseId, CancellationToken cancellationToken);
        void Update(Course course);
        void Delete(Course course);
    }
}
