using Domain.Entities;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext _context;

        public RatingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Rating> GetByUserAndCourseAsync(int userId, int courseId, CancellationToken cancellationToken)
        {
            return await _context.Ratings.FirstOrDefaultAsync(r => r.UserId == userId && r.CourseId == courseId, cancellationToken);
        }

        public async Task AddAsync(Rating rating, CancellationToken cancellationToken)
        {
            await _context.Ratings.AddAsync(rating, cancellationToken);
        }
        public async Task<Rating> GetByIdAsync(int ratingId, CancellationToken cancellationToken)
        {
            return await _context.Ratings
                .FirstOrDefaultAsync(r => r.RatingId == ratingId, cancellationToken);
        }

        public void Delete(Rating rating)
        {
            _context.Ratings.Remove(rating);
        }

        public void Update(Rating rating)
        {
            _context.Ratings.Update(rating);
        }

        public async Task<IEnumerable<Rating>> GetByCourseIdAsync(int courseId, CancellationToken cancellationToken)
        {
            return await _context.Ratings
                .Include(r => r.User)
                .Where(r => r.CourseId == courseId)
                .ToListAsync(cancellationToken);
        }
    }
}
