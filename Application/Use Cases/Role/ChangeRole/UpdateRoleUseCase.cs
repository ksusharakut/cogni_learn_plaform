using Application.Exceptions;
using Application.Use_Cases.Role.DTOs;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Use_Cases.Role.ChangeRole
{
    public class UpdateRoleUseCase : IUpdateRoleUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<RoleDTO> _validator;

        public UpdateRoleUseCase(IUnitOfWork unitOfWork, IValidator<RoleDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task ExecuteAsync(int id, RoleDTO roleDTO, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(roleDTO, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id, cancellationToken);
            if (role == null)
            {
                throw new NotFoundException($"Роль с идентификатором {id} не найдена.");
            }

            if (role.RoleName == roleDTO.RoleName)
            {
                throw new ConflictException("Новое имя роли совпадает с текущим.");
            }

            role.RoleName = roleDTO.RoleName;

            _unitOfWork.RoleRepository.Update(role);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
