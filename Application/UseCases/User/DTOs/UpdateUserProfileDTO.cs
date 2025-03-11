namespace Application.Use_Cases.User.DTOs
{
    public class UpdateUserProfileDTO
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateOnly DateBirth { get; set; }
        public string Email { get; set; }
    }
}
