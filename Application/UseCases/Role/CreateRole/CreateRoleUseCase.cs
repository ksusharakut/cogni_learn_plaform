using Application.Use_Cases.Role.DTOs;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Use_Cases.Role.CreateRole
{
    public class CreateRoleUseCase : ICreateRoleUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<RoleDTO> _validator;
        private readonly IMapper _mapper;

        public CreateRoleUseCase(IUnitOfWork unitOfWork,
            IValidator<RoleDTO> validator,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(RoleDTO roleDTO, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(roleDTO, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            roleDTO.RoleName = roleDTO.RoleName.ToLowerInvariant();

            var existingRole = await _unitOfWork.RoleRepository.GetByNameAsync(roleDTO.RoleName, cancellationToken);

            if (existingRole != null)
            {
                throw new ArgumentException($"Role with the name '{roleDTO.RoleName}' already exists.");
            }

            var role = _mapper.Map<Domain.Entities.Role>(roleDTO);

            await _unitOfWork.RoleRepository.AddAsync(role, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);  

        }
    }
}
