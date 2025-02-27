using Application.Common;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;

        public TokenService(IConfiguration configuration, IMemoryCache cache)
        {
            _configuration = configuration;
            _cache = cache;
        }

        public string GenerateAccessToken(User user)
        {
            if (user.Roles == null || !user.Roles.Any())
            {
                throw new InvalidOperationException("User has no roles assigned.");
            }

            Console.WriteLine($"Participant: Id={user.UserId}, Email={user.Email}, Roles={string.Join(", ", user.Roles.Select(r => r.RoleName))}");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }

        public User GetUserByRefreshToken(string refreshToken)
        {
            return _cache.Get<User>(refreshToken);
        }

        public void RemoveRefreshToken(string refreshToken)
        {
            _cache.Remove(refreshToken);
        }

        public void StoreRefreshToken(string refreshToken, User user)
        {
            var expiryTime = DateTime.UtcNow.AddDays(7);
            _cache.Set(refreshToken, user, expiryTime);
        }
    }
}
