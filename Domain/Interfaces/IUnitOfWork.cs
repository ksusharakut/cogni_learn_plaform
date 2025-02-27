using Domain.Interfaces.RepositoryInterfaces;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }
        IPasswordResetTokenRepository PasswordResetTokenRepository { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
