using Application.Common;
using Application.Exceptions;
using Application.Use_Cases.Auth.DTOs;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Use_Cases.Auth.Register
{
    public class RegisterUseCase : IRegisterUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<UserRegisterDTO> _validator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthSettings _authSettings;

        public RegisterUseCase(IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IValidator<UserRegisterDTO> validator, 
            IPasswordHasher passwordHasher,
            IAuthSettings authSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
            _passwordHasher = passwordHasher;
            _authSettings = authSettings;
        }

        public async Task ExecuteAsync(UserRegisterDTO request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingUser = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (existingUser != null)
            {
                throw new ConflictException("Такой email уже используется.");
            }

            var hashedPassword = _passwordHasher.HashPassword(request.Password);

            var participant = _mapper.Map<Domain.Entities.User>(request);
            participant.PasswordHash = hashedPassword;

            var userRole = await _unitOfWork.RoleRepository.GetByNameAsync(_authSettings.DefaultUserRole, cancellationToken);
            Console.WriteLine($"Role: Name={userRole?.RoleName}, Id={userRole?.RoleId}");
            if (userRole == null)
            {
                throw new NotFoundException($"Роль '{_authSettings.DefaultUserRole}' не найдена.");
            }

            participant.Roles = new List<Domain.Entities.Role> { userRole };

            await _unitOfWork.UserRepository.AddAsync(participant, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
