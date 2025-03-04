using Domain.Entities;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AnswerOptionRepository : IAnswerOptionRepository
    {
        private readonly ApplicationDbContext _context;

        public AnswerOptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AnswerOption answerOption, CancellationToken cancellationToken)
        {
            await _context.AnswerOptions.AddAsync(answerOption, cancellationToken);
        }

        public async Task<AnswerOption> GetByIdWithQuestionAsync(int answerOptionId, CancellationToken cancellationToken)
        {
            return await _context.AnswerOptions
                .Include(a => a.Question)
                    .ThenInclude(q => q.Lesson)
                        .ThenInclude(l => l.Chapter)
                            .ThenInclude(c => c.Course)
                .FirstOrDefaultAsync(a => a.AnswerOptionId == answerOptionId, cancellationToken);
        }

        public void Update(AnswerOption answerOption)
        {
            _context.AnswerOptions.Update(answerOption);
        }

        public void Delete(AnswerOption answerOption)
        {
            _context.AnswerOptions.Remove(answerOption);
        }

        public async Task<List<AnswerOption>> GetByQuestionIdAsync(int questionId, CancellationToken cancellationToken)
        {
            return await _context.AnswerOptions
                .Where(a => a.QuestionId == questionId)
                .ToListAsync(cancellationToken);
        }
    }
}
