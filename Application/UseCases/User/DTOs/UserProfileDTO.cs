﻿namespace Application.Use_Cases.User.UpdateUserProfile
{
    public class UserProfileDTO
    {
        public int UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateOnly DateBirth { get; set; }
        public string Email { get; set; }
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
