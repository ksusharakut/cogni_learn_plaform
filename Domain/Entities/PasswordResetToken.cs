using Domain.Enums;

namespace Domain.Entities
{
    public class PasswordResetToken 
    {
        public int PasswordResetTokenId { get; set; }
        public int UserId { get; set; }
        public string TokenHash { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public TokenStatus Status { get; set; }

        public virtual User User { get; set; }
    }
}
