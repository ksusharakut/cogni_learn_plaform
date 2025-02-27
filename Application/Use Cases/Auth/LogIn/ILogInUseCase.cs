using Application.Use_Cases.Auth.DTOs;

namespace Application.Use_Cases.Auth.LogIn
{
    public interface ILogInUseCase
    {
        Task<AuthResultDTO> ExecuteAsync(LogInUserDTO request, CancellationToken cancellationToken);
    }
}
