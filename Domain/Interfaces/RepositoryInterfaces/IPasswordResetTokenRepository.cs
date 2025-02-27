using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface IPasswordResetTokenRepository
    {
        Task AddAsync(PasswordResetToken token, CancellationToken cancellationToken);
        Task<PasswordResetToken> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken);
        Task UpdateAsync(PasswordResetToken token, CancellationToken cancellationToken);
        Task<List<PasswordResetToken>> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
    }
}
