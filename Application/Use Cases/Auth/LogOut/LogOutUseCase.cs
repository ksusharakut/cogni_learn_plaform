using Application.Common;
using Application.Exceptions;
using Application.Use_Cases.Auth.DTOs;
using Domain.Interfaces;

namespace Application.Use_Cases.Auth.LogOut
{
    public class LogOutUseCase : ILogOutUseCase
    {
        private readonly ITokenService _tokenService;

        public LogOutUseCase(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public async Task ExecuteAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var refreshToken = request.RefreshToken;
            var user = _tokenService.GetUserByRefreshToken(refreshToken);
            if (user == null)
            {
                throw new InvalidTokenException("Неверный или истёкший код восстановления.");
            }

            _tokenService.RemoveRefreshToken(refreshToken);
        }
    }
}
