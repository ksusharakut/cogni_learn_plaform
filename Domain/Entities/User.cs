using System.Text.Json.Serialization;
using System.Transactions;

namespace Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateOnly DateBirth { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        [JsonIgnore]
        public virtual ICollection<Role> Roles { get; set; }
        public ICollection<UserCourse> UserCourses { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<PasswordResetToken> PasswordResetTokens { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
