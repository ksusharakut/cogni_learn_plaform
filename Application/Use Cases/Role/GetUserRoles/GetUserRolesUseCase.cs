using Domain.Interfaces.RepositoryInterfaces;

namespace Application.Use_Cases.Role.GetUserRoles
{
    public class GetUserRolesUseCase : IGetUserRolesUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetUserRolesUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Domain.Entities.Role>> ExecuteAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return user.Roles;
        }
    }
}
