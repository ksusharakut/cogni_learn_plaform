using Domain.Entities;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Question question, CancellationToken cancellationToken)
        {
            await _context.Questions.AddAsync(question, cancellationToken);
        }

        public async Task<Question> GetByIdAsync(int questionId, CancellationToken cancellationToken)
        {
            return await _context.Questions
                .FirstOrDefaultAsync(q => q.QuestionId == questionId, cancellationToken);
        }

        public async Task<Question> GetByIdWithLessonAsync(int questionId, CancellationToken cancellationToken)
        {
            return await _context.Questions
                .Include(q => q.Lesson)
                    .ThenInclude(l => l.Chapter)
                        .ThenInclude(c => c.Course)
                .FirstOrDefaultAsync(q => q.QuestionId == questionId, cancellationToken);
        }

        public void Update(Question question)
        {
            _context.Questions.Update(question);
        }

        public void Delete(Question question)
        {
            _context.Questions.Remove(question);
        }

        public async Task<int> GetMaxOrderIndexAsync(int lessonId, CancellationToken cancellationToken)
        {
            var maxIndex = await _context.Questions
                .Where(q => q.LessonId == lessonId)
                .MaxAsync(q => (int?)q.OrderIndex, cancellationToken) ?? 0;
            return maxIndex;
        }

        public async Task<List<Question>> GetByLessonIdAsync(int lessonId, CancellationToken cancellationToken)
        {
            return await _context.Questions
                .Where(q => q.LessonId == lessonId)
                .OrderBy(q => q.OrderIndex)
                .ToListAsync(cancellationToken);
        }
    }
}
