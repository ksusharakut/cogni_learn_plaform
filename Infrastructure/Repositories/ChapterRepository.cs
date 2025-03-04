using Domain.Entities;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly ApplicationDbContext _context;

        public ChapterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Chapter chapter, CancellationToken cancellationToken)
        {
            await _context.Chapters.AddAsync(chapter, cancellationToken);
        }

        public void Delete(Chapter chapter)
        {
            _context.Chapters.Remove(chapter);
        }

        public async Task<int> GetMaxOrderIndexAsync(int courseId, CancellationToken cancellationToken)
        {
            var maxIndex = await _context.Chapters
                .Where(c => c.CourseId == courseId)
                .MaxAsync(c => (int?)c.OrderIndex, cancellationToken) ?? 0;
            return maxIndex;
        }

        public void Update(Chapter chapter)
        {
            _context.Chapters.Update(chapter);
        }

        public async Task<Chapter> GetByIdWithCourseAsync(int chapterId, CancellationToken cancellationToken)
        {
            return await _context.Chapters.Include(c => c.Course).FirstOrDefaultAsync(c => c.ChapterId == chapterId, cancellationToken);
        }

        public async Task<List<Chapter>> GetByCourseIdAsync(int courseId, CancellationToken cancellationToken)
        {
            return await _context.Chapters
                .Where(c => c.CourseId == courseId)
                .ToListAsync(cancellationToken);
        }
    }
}
