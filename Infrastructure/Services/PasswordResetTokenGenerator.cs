using Application.Common;

namespace Infrastructure.Services
{
    public class PasswordResetTokenGenerator : IPasswordResetTokenGenerator
    {
        private readonly IPasswordHasher _passwordHasher;

        public PasswordResetTokenGenerator(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string GenerateToken()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString(); 
        }

        public string HashToken(string token)
        {
            return _passwordHasher.HashPassword(token);
        }

        public bool VerifyToken(string token, string tokenHash)
        {
            return _passwordHasher.VerifyPassword(token, tokenHash);
        }
    }
}
