using Domain.Entities;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly ApplicationDbContext _context;

        public LessonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Lesson lesson, CancellationToken cancellationToken)
        {
            await _context.Lessons.AddAsync(lesson, cancellationToken);
        }

        public async Task<Lesson> GetByIdAsync(int lessonId, CancellationToken cancellationToken)
        {
            return await _context.Lessons
                .FirstOrDefaultAsync(l => l.LessonId == lessonId, cancellationToken);
        }

        public async Task<Lesson> GetByIdWithChapterAsync(int lessonId, CancellationToken cancellationToken)
        {
            return await _context.Lessons
                .Include(l => l.Chapter)
                    .ThenInclude(c => c.Course)
                .FirstOrDefaultAsync(l => l.LessonId == lessonId, cancellationToken);
        }

        public void Update(Lesson lesson)
        {
            _context.Lessons.Update(lesson);
        }

        public void Delete(Lesson lesson)
        {
            _context.Lessons.Remove(lesson);
        }

        public async Task<int> GetMaxOrderIndexAsync(int chapterId, CancellationToken cancellationToken)
        {
            var maxIndex = await _context.Lessons
                .Where(l => l.ChapterId == chapterId)
                .MaxAsync(l => (int?)l.OrderIndex, cancellationToken) ?? 0;
            return maxIndex;
        }

        public async Task<List<Lesson>> GetByChapterIdAsync(int chapterId, CancellationToken cancellationToken)
        {
            return await _context.Lessons
                .Where(l => l.ChapterId == chapterId)
                .OrderBy(l => l.OrderIndex)
                .ToListAsync(cancellationToken);
        }
    }
}
