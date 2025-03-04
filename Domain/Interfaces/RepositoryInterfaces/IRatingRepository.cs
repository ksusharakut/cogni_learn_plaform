using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface IRatingRepository
    {
        Task<Rating> GetByUserAndCourseAsync(int userId, int courseId, CancellationToken cancellationToken);
        Task AddAsync(Rating rating, CancellationToken cancellationToken);
        Task<Rating> GetByIdAsync(int ratingId, CancellationToken cancellationToken); 
        void Delete(Rating rating);
        void Update(Rating rating);
        Task<IEnumerable<Rating>> GetByCourseIdAsync(int courseId, CancellationToken cancellationToken);
    }
}
