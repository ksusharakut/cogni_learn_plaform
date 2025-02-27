using Application.Common;
using Application.Exceptions;
using Application.Use_Cases.Auth.DTOs;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Use_Cases.Auth.SetNewPassword
{
    public class SetNewPasswordUseCase : ISetNewPasswordUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<SetNewPasswordRequest> _validator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPasswordResetTokenGenerator _tokenGenerator;

        public SetNewPasswordUseCase(
            IUnitOfWork unitOfWork,
            IValidator<SetNewPasswordRequest> validator,
            IPasswordHasher passwordHasher,
            IPasswordResetTokenGenerator tokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
        }
        public async Task ExecuteAsync(SetNewPasswordRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("Пользователь не найден.");
            }

            var tokenHash = _tokenGenerator.HashToken(request.ResetToken);

            var userTokens = await _unitOfWork.PasswordResetTokenRepository.GetByUserIdAsync(user.UserId, cancellationToken);
            var resetToken = userTokens.FirstOrDefault(t => _tokenGenerator.VerifyToken(request.ResetToken, t.TokenHash));
            if (resetToken == null || resetToken.UserId != user.UserId)
            {
                throw new InvalidTokenException("Неверный код восстановления.");
            }

            if (resetToken.Status != TokenStatus.Active)
            {
                throw new InvalidTokenException("Код восстановления уже использован или истёк.");
            }

            var tokenExpiry = resetToken.CreatedAt.AddHours(1);
            if (DateTimeOffset.UtcNow > tokenExpiry)
            {
                resetToken.Status = TokenStatus.Expired;
                await _unitOfWork.PasswordResetTokenRepository.UpdateAsync(resetToken, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                throw new InvalidTokenException("Срок действия кода восстановления истек.");
            }

            user.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
            resetToken.Status = TokenStatus.Used;

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.PasswordResetTokenRepository.UpdateAsync(resetToken, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
