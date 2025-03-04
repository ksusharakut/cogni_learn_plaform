using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface IUserCourseRepository
    {
        Task<UserCourse> GetByUserAndCourseAsync(int userId, int courseId, CancellationToken cancellationToken);
        Task AddAsync(UserCourse userCourse, CancellationToken cancellationToken);
        Task<IEnumerable<Course>> GetCoursesByUserIdAsync(int userId, CancellationToken cancellationToken); 
    }
}
