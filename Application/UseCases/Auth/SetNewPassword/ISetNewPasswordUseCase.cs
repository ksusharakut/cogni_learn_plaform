using Application.Use_Cases.Auth.DTOs;

namespace Application.Use_Cases.Auth.SetNewPassword
{
    public interface ISetNewPasswordUseCase
    {
        Task ExecuteAsync(SetNewPasswordRequest request, CancellationToken cancellationToken);
    }
}
