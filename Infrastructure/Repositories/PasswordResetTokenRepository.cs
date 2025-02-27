using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public PasswordResetTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PasswordResetToken token, CancellationToken cancellationToken)
        {
            await _context.PasswordResetTokens.AddAsync(token, cancellationToken);
        }

        public async Task<PasswordResetToken> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken)
        {
            return await _context.PasswordResetTokens.FirstOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken);
        }

        public async Task UpdateAsync(PasswordResetToken token, CancellationToken cancellationToken)
        {
            _context.PasswordResetTokens.Update(token);
        }

        public async Task<List<PasswordResetToken>> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _context.PasswordResetTokens.Where(t => t.UserId == userId && t.Status == TokenStatus.Active).ToListAsync(cancellationToken);
        }
    }
}
