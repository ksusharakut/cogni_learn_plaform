using Domain.Entities;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Course course, CancellationToken cancellationToken)
        {
            await _context.Courses.AddAsync(course, cancellationToken);
        }

        public async Task<Course?> GetByIdAsync(int courseId, CancellationToken cancellationToken)
        {
            return await _context.Courses
                .Include(c => c.Categories)
                .Include(c => c.Ratings)
                .Include(c => c.Chapters)
                .FirstOrDefaultAsync(c => c.CourseId == courseId, cancellationToken);
        }

        public async Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Courses
                .Include(c => c.Categories)
                .Include(c => c.Ratings)
                .Include(c => c.Chapters)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Course>> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _context.Courses
                .Where(c => c.UserId == userId)
                .Include(c => c.Categories)
                .Include(c => c.Ratings)
                .Include(c => c.Chapters)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Category>> GetByIdsAsync(IEnumerable<int> categoryIds, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .Where(c => categoryIds.Contains(c.CategoryId))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Course>> GetAllPublishedAsync(CancellationToken cancellationToken)
        {
            return await _context.Courses
                .Where(c => c.IsPublished)
                .Include(c => c.User)          
                .Include(c => c.Categories)    
                .ToListAsync(cancellationToken);
        }

        public async Task<Course> GetByIdWithDetailsAsync(int courseId, CancellationToken cancellationToken)
        {
            return await _context.Courses
                .Include(c => c.User)
                .Include(c => c.Categories)
                .Include(c => c.Chapters)
                    .ThenInclude(ch => ch.Lessons)
                        .ThenInclude(l => l.Questions) 
                .FirstOrDefaultAsync(c => c.CourseId == courseId, cancellationToken);
        }

        public void Update(Course course)
        {
            _context.Courses.Update(course);
        }

        public void Delete(Course course)
        {
            _context.Courses.Remove(course);
        }
    }
}
