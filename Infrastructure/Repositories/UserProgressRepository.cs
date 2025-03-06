using Domain.Entities;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserProgressRepository : IUserProgressRepository
    {
        private readonly ApplicationDbContext _context;

        public UserProgressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserProgress progress, CancellationToken cancellationToken)
        {
            await _context.UserProgress.AddAsync(progress, cancellationToken);
        }

        public void Update(UserProgress progress)
        {
            _context.UserProgress.Update(progress);
        }

        public async Task<UserProgress> GetByUserAndChapterAsync(int userId, int chapterId, CancellationToken cancellationToken)
        {
            return await _context.UserProgress
                .FirstOrDefaultAsync(up => up.UserId == userId && up.ChapterId == chapterId && !up.LessonId.HasValue, cancellationToken);
        }

        public async Task<UserProgress> GetByUserAndLessonAsync(int userId, int lessonId, CancellationToken cancellationToken)
        {
            return await _context.UserProgress
                .FirstOrDefaultAsync(up => up.UserId == userId && up.LessonId == lessonId, cancellationToken);
        }

        public async Task<UserProgress> GetByUserAndQuestionAsync(int userId, int questionId, CancellationToken cancellationToken)
        {
            return await _context.UserProgress
                .FirstOrDefaultAsync(up => up.UserId == userId && up.QuestionId == questionId, cancellationToken);
        }

        public async Task<List<UserProgress>> GetByUserAndCourseAsync(int userId, int courseId, CancellationToken cancellationToken)
        {
            return await _context.UserProgress
                .Where(up => up.UserId == userId && up.CourseId == courseId)
                .ToListAsync(cancellationToken);
        }

    }
}
