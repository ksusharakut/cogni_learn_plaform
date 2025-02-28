using Domain.Entities;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Role role, CancellationToken cancellationToken)
        {
            await _context.AddAsync(role, cancellationToken);
        }

        public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Roles.ToListAsync(cancellationToken);
        }

        public async Task<Role> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == id, cancellationToken);
        }

        public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == name, cancellationToken);
        }

        public void Update(Role role)
        {
            _context.Roles.Update(role);
        }
    }
}
