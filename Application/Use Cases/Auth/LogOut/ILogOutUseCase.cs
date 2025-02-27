using Application.Use_Cases.Auth.DTOs;

namespace Application.Use_Cases.Auth.LogOut
{
    public interface ILogOutUseCase
    {
        Task ExecuteAsync(RefreshTokenRequest refreshToken, CancellationToken cancellationToken);
    }
}
