namespace Application.Use_Cases.Role.AssignRole
{
    public interface IAssignRoleUseCase
    {
        Task ExecuteAsync(int userId, int roleId, CancellationToken cancellationToken);
    }
}
