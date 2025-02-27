using Application.Common;
using Application.Exceptions;
using Application.Use_Cases.Auth.DTOs;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Use_Cases.Auth.LogIn
{
    public class LogInUseCase : ILogInUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IValidator<LogInUserDTO> _validator;

        public LogInUseCase(IUnitOfWork unitOfWork, ITokenService tokenService,
            IPasswordHasher passwordHasher, IValidator<LogInUserDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _validator = validator;
        }

        public async Task<AuthResultDTO> ExecuteAsync(LogInUserDTO request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new AuthenticationFailedException("Неверный логин или пароль.");
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            _tokenService.StoreRefreshToken(refreshToken, user);

            return new AuthResultDTO { AccessToken = accessToken, RefreshToken = refreshToken };
        }
    }
}
