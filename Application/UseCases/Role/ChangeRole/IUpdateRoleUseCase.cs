using Application.Use_Cases.Role.DTOs;

namespace Application.Use_Cases.Role.ChangeRole
{
    public interface IUpdateRoleUseCase
    {
        Task ExecuteAsync(int id, RoleDTO roleDTO, CancellationToken cancellationToken);
    }
}
