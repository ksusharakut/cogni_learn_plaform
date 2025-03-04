using Domain.Entities;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserCourseRepository : IUserCourseRepository
    {
        private readonly ApplicationDbContext _context;

        public UserCourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserCourse> GetByUserAndCourseAsync(int userId, int courseId, CancellationToken cancellationToken)
        {
            return await _context.UserCourses.FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CourseId == courseId, cancellationToken);
        }

        public async Task AddAsync(UserCourse userCourse, CancellationToken cancellationToken)
        {
            await _context.UserCourses.AddAsync(userCourse, cancellationToken);
        }

        public async Task<IEnumerable<Course>> GetCoursesByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _context.UserCourses
                .Where(uc => uc.UserId == userId)
                .Include(uc => uc.Course)
                    .ThenInclude(c => c.User) 
                .Include(uc => uc.Course)
                    .ThenInclude(c => c.Categories) 
                .Select(uc => uc.Course)
                .ToListAsync(cancellationToken);
        }
    }
}
