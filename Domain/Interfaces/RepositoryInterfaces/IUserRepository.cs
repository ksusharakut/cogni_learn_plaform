using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
        void Update(User user);
    }
}
