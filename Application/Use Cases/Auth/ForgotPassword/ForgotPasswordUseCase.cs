using Application.Common;
using Application.Use_Cases.Auth.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Use_Cases.Auth.ForgotPassword
{
    public class ForgotPasswordUseCase : IForgotPasswordUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<EmailDTO> _validator;
        private readonly IEmailService _emailService;
        private readonly IPasswordResetTokenGenerator _tokenGenerator;

        public ForgotPasswordUseCase(
            IUnitOfWork unitOfWork,
            IValidator<EmailDTO> validator,
            IEmailService emailService,
            IPasswordResetTokenGenerator tokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _emailService = emailService;
            _tokenGenerator = tokenGenerator;
        }

        public async Task ExecuteAsync(EmailDTO request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null)
            {
                // Тихий выход для безопасности
                return;
            }

            var resetToken = _tokenGenerator.GenerateToken();
            var tokenHash = _tokenGenerator.HashToken(resetToken); 

            var passwordResetToken = new PasswordResetToken
            {
                UserId = user.UserId, 
                TokenHash = tokenHash,
                CreatedAt = DateTimeOffset.UtcNow,
                Status = TokenStatus.Active
            };

            await _unitOfWork.PasswordResetTokenRepository.AddAsync(passwordResetToken, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var emailSubject = "Запрос на сброс пароля";
            var emailBody = $"Ваш код сброса пароля: {resetToken}. Он действителен в течение 1 часа.";
            await _emailService.SendEmailAsync(request.Email, emailSubject, emailBody, cancellationToken);
        }
    }
}

