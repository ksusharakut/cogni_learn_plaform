using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Use_Cases.Role.RemoveRole
{
    public class RemoveRoleUseCase : IRemoveRoleUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveRoleUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(int userId, int roleId, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("Пользователь не найден.");
            }

            var role = await _unitOfWork.RoleRepository.GetByIdAsync(roleId, cancellationToken);
            if (role == null)
            {
                throw new NotFoundException("Роль не найдена.");
            }

            var userRole = user.Roles.FirstOrDefault(ur => ur.RoleId == roleId);
            if (userRole == null)
            {
                throw new ConflictException("У пользователя нет этой роли.");
            }

            user.Roles.Remove(userRole);

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
