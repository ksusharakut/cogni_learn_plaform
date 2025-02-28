using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface IRoleRepository
    {
        Task AddAsync(Role role, CancellationToken cancellationToken);
        Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken);
        Task<Role> GetByIdAsync(int id, CancellationToken cancellationToken);
        void Update(Role role);
    }
}
