using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        void Update(User user);
    }
}
