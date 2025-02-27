using Domain.Entities;

namespace Application.Common
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        void StoreRefreshToken(string refreshToken, User user);
        User GetUserByRefreshToken(string refreshToken);
        void RemoveRefreshToken(string refreshToken);
    }
}
