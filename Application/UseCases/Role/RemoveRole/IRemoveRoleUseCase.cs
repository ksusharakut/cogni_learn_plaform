namespace Application.Use_Cases.Role.RemoveRole
{
    public interface IRemoveRoleUseCase
    {
        Task ExecuteAsync(int userId, int roleId, CancellationToken cancellationToken);
    }
}
