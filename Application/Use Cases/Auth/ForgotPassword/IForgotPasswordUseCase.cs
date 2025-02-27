using Application.Use_Cases.Auth.DTOs;

namespace Application.Use_Cases.Auth.ForgotPassword
{
    public interface IForgotPasswordUseCase
    {
        Task ExecuteAsync(EmailDTO request, CancellationToken cancellationToken);
    }
}
