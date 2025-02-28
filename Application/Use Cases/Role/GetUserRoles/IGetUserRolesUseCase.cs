namespace Application.Use_Cases.Role.GetUserRoles
{
    public interface IGetUserRolesUseCase
    {
        Task<IEnumerable<Domain.Entities.Role>> ExecuteAsync(int userId, CancellationToken cancellationToken);
    }
}
