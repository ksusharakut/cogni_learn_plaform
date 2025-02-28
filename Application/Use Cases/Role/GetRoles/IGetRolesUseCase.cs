using Domain.Entities;

namespace Application.Use_Cases.Role.GetRoles
{
    public interface IGetRolesUseCase
    {
        Task<IEnumerable<Domain.Entities.Role>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
