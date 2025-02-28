using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Use_Cases.Role.AssignRole
{
    public class AssignRoleUseCase : IAssignRoleUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssignRoleUseCase(IUnitOfWork unitOfWork)
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

            if (user.Roles.Any(ur => ur.RoleId == roleId))
            {
                throw new ConflictException("Эта роль уже назначена пользователю.");
            }

            user.Roles.Add(role);

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}

