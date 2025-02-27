using Application.Common;
using Application.Exceptions;
using Application.Use_Cases.Auth.DTOs;

namespace Application.Use_Cases.Auth.RefreshToken
{
    public class RefreshTokenUseCase : IRefreshTokenUseCase
    {
        private readonly ITokenService _tokenService;
         
        public RefreshTokenUseCase(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<AuthResultDTO> ExecuteAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var refreshToken = request.RefreshToken;
            var user = _tokenService.GetUserByRefreshToken(refreshToken);
            if (user == null)
            {
                throw new InvalidTokenException("Неверный или истёкший код восстановления.");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            _tokenService.RemoveRefreshToken(refreshToken);
            _tokenService.StoreRefreshToken(newRefreshToken, user);

            return new AuthResultDTO { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
        }
    }
}
