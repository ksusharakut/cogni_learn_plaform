using Application.Common;
using Application.Exceptions;
using Application.Use_Cases.User.DTOs;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.User.ChangePassword
{
    public class ChangePasswordUseCase : IChangePasswordUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ChangePasswordDTO> _validator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChangePasswordUseCase(
            IUnitOfWork unitOfWork,
            IValidator<ChangePasswordDTO> validator,
            IPasswordHasher passwordHasher,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ExecuteAsync(ChangePasswordDTO request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new AuthenticationFailedException("Unable to identify the current user.");
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            if (!_passwordHasher.VerifyPassword(request.OldPassword, user.PasswordHash))
            {
                throw new AuthenticationFailedException("Old password is incorrect.");
            }

            user.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

    }
}
