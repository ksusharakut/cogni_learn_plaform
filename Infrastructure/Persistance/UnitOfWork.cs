using Domain.Interfaces;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;

namespace Infrastructure.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IRoleRepository RoleRepository { get; set; }

        public IUserRepository UserRepository { get; set; }

        public IPasswordResetTokenRepository PasswordResetTokenRepository { get; set; }

        public UnitOfWork(ApplicationDbContext context,
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IPasswordResetTokenRepository passwordResetTokenRepository)
        {
            _context = context;
            RoleRepository = roleRepository;
            UserRepository = userRepository;
            PasswordResetTokenRepository = passwordResetTokenRepository;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
