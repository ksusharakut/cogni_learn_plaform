namespace Application.Common
{
    public interface IPasswordResetTokenGenerator
    {
        string GenerateToken();
        string HashToken(string token);
        bool VerifyToken(string token, string tokenHash);
    }
}
