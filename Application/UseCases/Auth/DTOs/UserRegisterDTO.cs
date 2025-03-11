namespace Application.Use_Cases.Auth.DTOs
{
    public class UserRegisterDTO
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateOnly DateBirth { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
