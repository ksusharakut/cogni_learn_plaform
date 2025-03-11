using Application.Use_Cases.Auth.DTOs;

namespace Application.Use_Cases.Auth.RefreshToken
{
    public interface IRefreshTokenUseCase
    {
        Task<AuthResultDTO> ExecuteAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
    }
}
