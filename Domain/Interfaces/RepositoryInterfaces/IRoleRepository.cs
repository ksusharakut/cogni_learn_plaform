using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface IRoleRepository
    {
        Task AddAsync(Role role, CancellationToken cancellationToken);
        Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
